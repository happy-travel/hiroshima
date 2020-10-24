using CSharpFunctionalExtensions;
using HappyTravel.AmazonS3Client.Services;
using HappyTravel.Hiroshima.Common.Infrastructure.Extensions;
using HappyTravel.Hiroshima.Common.Infrastructure.Utilities;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.Data.Extensions;
using HappyTravel.Hiroshima.DirectManager.Infrastructure.Extensions;
using HappyTravel.Hiroshima.DirectManager.Models.Internal;
using HappyTravel.Hiroshima.DirectManager.RequestValidators;
using Imageflow.Fluent;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public class ImageManagementService : IImageManagementService
    {
        public ImageManagementService(IContractManagerContextService contractManagerContextService,
            DirectContractsDbContext dbContext, IAmazonS3ClientService amazonS3ClientService, IOptions<ImageManagementServiceOptions> options)
        {
            _contractManagerContext = contractManagerContextService;
            _dbContext = dbContext;
            _amazonS3ClientService = amazonS3ClientService;
            _bucketName = options.Value.AmazonS3Bucket;
            _regionEndpoint = options.Value.AmazonS3RegionEndpoint;
        }


        public Task<Result<List<Models.Responses.SlimImage>>> Get(int accommodationId)
        {
            return _contractManagerContext.GetContractManager()
                .EnsureAccommodationBelongsToContractManager(_dbContext, accommodationId)
                .Map(async contractManager =>
                {
                    return await _dbContext.Images
                        .Where(image => image.ContractManagerId == contractManager.Id && image.AccommodationId == accommodationId).OrderBy(image => image.Position).ToListAsync();
                })
                .Map(images => Build(images));
        }


        public Task<Result<Models.Responses.Image>> Add(Models.Requests.Image image)
        {
            return _contractManagerContext.GetContractManager()
                .Bind(contractManager =>
                {
                    var validationResult = ValidationHelper.Validate(image, new ImageValidator());

                    return validationResult.IsFailure ? Result.Failure<ContractManager>(validationResult.Error) : Result.Success(contractManager);
                })
                .Map(contractManager => Create(contractManager.Id, image))
                .Ensure(dbImage => ValidateImageType(image.UploadedFile).Value, "Invalid image file type")
                .Bind(dbImage => AppendContent(dbImage, image.UploadedFile))
                .Ensure(maybeImage => maybeImage.HasValue, "Error saving image")
                .Map(maybeImage => Build(maybeImage));


            async Task<Result<Maybe<Image>>> AppendContent(Image dbImage, FormFile uploadedFile)
            {
                using BinaryReader binaryReader = new BinaryReader(uploadedFile.OpenReadStream());
                var imageBytes = binaryReader.ReadBytes((int)uploadedFile.Length);

                var validationResult = ValidateImageDimensions(imageBytes);
                if (validationResult.Result.IsFailure)
                    return Result.Failure<Maybe<Image>>(validationResult.Result.Error);

                var imageSet = await ConvertImage(imageBytes);
                if (imageSet.LargeImage == null || imageSet.SmallImage == null)
                    return Maybe<Image>.None;

                dbImage.Position = _dbContext.Images.Count(image => image.AccommodationId == dbImage.AccommodationId);

                var entry = _dbContext.Images.Add(dbImage);

                await _dbContext.SaveChangesAsync();

                dbImage.LargeImageKey = $"{S3FolderName}/{dbImage.AccommodationId}/{entry.Entity.Id}-large.jpg";
                dbImage.SmallImageKey = $"{S3FolderName}/{dbImage.AccommodationId}/{entry.Entity.Id}-small.jpg";

                await using var largeStream = new MemoryStream(imageSet.LargeImage);
                await using var smallStream = new MemoryStream(imageSet.SmallImage);
                var imageList = new List<(string key, Stream stream)>
                {
                    ( dbImage.LargeImageKey, largeStream ),
                    ( dbImage.SmallImageKey, smallStream )
                };

                var resultList = await _amazonS3ClientService.Add(_bucketName, imageList);
                foreach (var result in resultList)
                {
                    if (result.IsFailure)
                    {
                        _dbContext.Images.Remove(entry.Entity);

                        await _dbContext.SaveChangesAsync();

                        return Maybe<Image>.None;
                    }
                }

                await _dbContext.SaveChangesAsync();

                _dbContext.DetachEntry(entry.Entity);

                return Maybe<Image>.From(entry.Entity);
            }

            Result<bool> ValidateImageType(FormFile uploadedFile)
            {
                var extension = uploadedFile != null ? Path.GetExtension(uploadedFile.FileName).ToLower() : string.Empty;
                return ((extension == ".jpg") || (extension == ".jpeg") || (extension == ".png"));
            }

            async Task<Result> ValidateImageDimensions(byte[] imageBytes)
            {
                var info = await ImageJob.GetImageInfo(new BytesSource(imageBytes));

                // Validation image size
                if ((info.ImageWidth < MinimumImageWidth) || (info.ImageHeight < MinimumImageHeight))
                    return Result.Failure($"Uploading picture size must be at least {MinimumImageWidth}×{MinimumImageHeight} pixels");

                // Validation image dimension difference
                if (info.ImageWidth / info.ImageHeight > 2)
                    return Result.Failure("Uploading picture width is more than 2 times the height");

                if (info.ImageHeight / info.ImageWidth > 2)
                    return Result.Failure("Uploading picture height is more than 2 times the width");

                return Result.Success();
            }

            async Task<ImageSet> ConvertImage(byte[] imageBytes)
            {
                var imagesSet = new ImageSet();

                using var imageJob = new ImageJob();
                var jobResult = await imageJob.Decode(imageBytes)
                    .Constrain(new Constraint(ConstraintMode.Within, ResizedLargeImageMaximumSideSize, ResizedLargeImageMaximumSideSize))
                    .Branch(f => f.ConstrainWithin(ResizedSmallImageMaximumSideSize, ResizedSmallImageMaximumSideSize).EncodeToBytes(new MozJpegEncoder(TargetJpegQuality, true)))
                    .EncodeToBytes(new MozJpegEncoder(TargetJpegQuality, true))
                    .Finish().InProcessAsync();

                imagesSet.SmallImage = (jobResult != null && jobResult.TryGet(1) != null && jobResult.TryGet(1).TryGetBytes() != null) 
                    ? jobResult.TryGet(1).TryGetBytes().Value.ToArray() 
                    : null;
                imagesSet.LargeImage = (jobResult != null && jobResult.TryGet(1) != null && jobResult.TryGet(1).TryGetBytes() != null) 
                    ?jobResult.TryGet(2).TryGetBytes().Value.ToArray()
                    : null;

                return imagesSet;
            }
        }


        public Task<Result> Update(int accommodationId, List<Models.Requests.SlimImage> images)
        {
            return _contractManagerContext.GetContractManager()
                .EnsureAccommodationBelongsToContractManager(_dbContext, accommodationId)
                .Bind(async contractManager =>
                {
                    var dbImages = await _dbContext.Images
                        .Where(image => image.ContractManagerId == contractManager.Id && image.AccommodationId == accommodationId).ToListAsync();

                    for (int i = 0; i < images.Count; i++)
                    {
                        var dbImage = dbImages.SingleOrDefault(image => image.Id == images[i].Id);
                        if (dbImage != null)
                        {
                            dbImage.Position = i;
                            dbImage.Description = JsonDocumentUtilities.CreateJDocument(images[i].Description);
                            
                            _dbContext.Images.Update(dbImage);
                        }
                    }
                    await _dbContext.SaveChangesAsync();

                    return Result.Success();
                });
        }


        public async Task<Result> Remove(int accommodationId, Guid imageId)
        {
            return await _contractManagerContext.GetContractManager()
                .Tap(async contractManager => 
                { 
                    var result = await RemoveImage(contractManager.Id, accommodationId, imageId);
                    return result ? Result.Success(contractManager) : Result.Failure<ContractManager>("Image deletion error");
                });
        }


        public async Task<Result> RemoveAll(int contractManagerId, int accommodationId)
        {
            var images = _dbContext.Images.Where(image => image.ContractManagerId == contractManagerId && image.AccommodationId == accommodationId);
            foreach (var image in images)
            {
                var result = await RemoveImage(contractManagerId, accommodationId, image.Id);
                if (!result)
                    return Result.Failure("Image deletion error");
            }
            return Result.Success();
        }


        private Image Create(int contractManagerId, Models.Requests.Image image) => new Image
        {
            AccommodationId = image.AccommodationId,
            OriginalName = image.UploadedFile.FileName,
            OriginalContentType = image.UploadedFile.ContentType,
            LargeImageKey = string.Empty,
            SmallImageKey = string.Empty,
            ContractManagerId = contractManagerId,
            Created = DateTime.UtcNow,
            Description = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string> { Ar = string.Empty, En = string.Empty, Ru = string.Empty } )
        };


        private List<Models.Responses.SlimImage> Build(List<Image> images)
        {
            var slimImages = new List<Models.Responses.SlimImage>();
            foreach (Image image in images)
            { 
                var slimImage = new Models.Responses.SlimImage
                    (
                        image.Id,
                        $"https://{_bucketName}.s3-{_regionEndpoint}.amazonaws.com/{image.LargeImageKey}",
                        $"https://{_bucketName}.s3-{_regionEndpoint}.amazonaws.com/{image.SmallImageKey}",
                        image.Description.GetValue<MultiLanguage<string>>()
                    ); 
                slimImages.Add(slimImage);
            }
            return slimImages;
        }


        private Models.Responses.Image Build(Maybe<Image> image)
            => new Models.Responses.Image(image.Value.Id, image.Value.OriginalName, image.Value.OriginalContentType, 
                image.Value.LargeImageKey, image.Value.SmallImageKey, image.Value.AccommodationId, image.Value.Position);


        private async Task<bool> RemoveImage(int contractManagerId, int accommodationId, Guid imageId)
        {
            var image = await _dbContext.Images.SingleOrDefaultAsync(c => c.ContractManagerId == contractManagerId && c.AccommodationId == accommodationId && c.Id == imageId);
            if (image is null)
                return false;

            var result = await _amazonS3ClientService.Delete(_bucketName, image.LargeImageKey);
            if (result.IsFailure)
                return false;

            result = await _amazonS3ClientService.Delete(_bucketName, image.SmallImageKey);
            if (result.IsFailure)
                return false;

            _dbContext.Images.Remove(image);

            await _dbContext.SaveChangesAsync();

            return true;
        }


        private const string S3FolderName = "images";
        private const long MinimumImageWidth = 500;
        private const long MinimumImageHeight = 300;
        private const int ResizedLargeImageMaximumSideSize = 1600;
        private const int ResizedSmallImageMaximumSideSize = 400;
        private const int TargetJpegQuality = 85;


        private readonly IContractManagerContextService _contractManagerContext;
        private readonly DirectContractsDbContext _dbContext;
        private readonly IAmazonS3ClientService _amazonS3ClientService;
        private readonly string _bucketName;
        private readonly string _regionEndpoint;
    }
}
