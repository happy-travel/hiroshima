using CSharpFunctionalExtensions;
using HappyTravel.AmazonS3Client.Services;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.Data.Extensions;
using HappyTravel.Hiroshima.DirectManager.Models.Internal;
using HappyTravel.Hiroshima.DirectManager.RequestValidators;
using Imageflow.Fluent;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.IO;
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
                byte[] imageBytes = null;
                using BinaryReader binaryReader = new BinaryReader(uploadedFile.OpenReadStream());
                imageBytes = binaryReader.ReadBytes((int)uploadedFile.Length);

                var validationResult = ValidateImageDimensions(imageBytes);
                if (validationResult.Result.IsFailure)
                    return Result.Failure<Maybe<Image>>(validationResult.Result.Error);

                var imageSet = await ConvertImage(imageBytes);

                var extension = Path.GetExtension(uploadedFile.FileName);
                dbImage.Created = DateTime.UtcNow;

                var entry = _dbContext.Images.Add(dbImage);

                await _dbContext.SaveChangesAsync();

                entry.Entity.LargeImageKey = $"{S3FolderName}/{dbImage.AccommodationId}/{entry.Entity.Id}-large.jpg";

                using var largeStream = new MemoryStream(imageSet.LargeImage);
                var result = await _amazonS3ClientService.Add(_bucketName, dbImage.LargeImageKey, largeStream);
                if (result.IsFailure)
                {
                    _dbContext.Images.Remove(entry.Entity);

                    await _dbContext.SaveChangesAsync();

                    return Maybe<Image>.None;
                }

                entry.Entity.SmallImageKey = $"{S3FolderName}/{dbImage.AccommodationId}/{entry.Entity.Id}-small.jpg";

                using var smallStream = new MemoryStream(imageSet.SmallImage);
                result = await _amazonS3ClientService.Add(_bucketName, dbImage.SmallImageKey, smallStream);
                if (result.IsFailure)
                {
                    _dbContext.Images.Remove(entry.Entity);

                    await _dbContext.SaveChangesAsync();

                    return Maybe<Image>.None;
                }

                await _dbContext.SaveChangesAsync();

                _dbContext.DetachEntry(entry.Entity);

                return Maybe<Image>.From(entry.Entity);
            }

            Result<bool> ValidateImageType(FormFile uploadedFile)
            {
                var extension = Path.GetExtension(uploadedFile.FileName).ToLower();
                return ((extension == ".jpg") || (extension == ".jpeg") || (extension == ".png"));
            }

            async Task<Result> ValidateImageDimensions(byte[] imageBytes)
            {
                var info = await ImageJob.GetImageInfo(new BytesSource(imageBytes));

                // Validation image size
                if ((info.ImageWidth < MinimumImageWidth) || (info.ImageHeight < MinimumImageHeight))
                    return Result.Failure<byte[]>("Uploading picture size is less than the minimum");

                // Validation image dimension difference
                if (info.ImageWidth / info.ImageHeight > 2)
                    return Result.Failure<byte[]>("Uploading picture width is more than 2 times the height");

                if (info.ImageHeight / info.ImageWidth > 2)
                    return Result.Failure<byte[]>("Uploading picture height is more than 2 times the width");

                return Result.Success();
            }

            async Task<ImageSet> ConvertImage(byte[] imageBytes)
            {
                ImageSet imagesSet = new ImageSet();

                using var imageJob = new ImageJob();
                var jobResult = await imageJob.Decode(imageBytes)
                    .Constrain(new Constraint(ConstraintMode.Within, MaximumSideSizeLarge, MaximumSideSizeLarge))
                    .Branch(f => f.ConstrainWithin(MaximumSideSizeSmall, MaximumSideSizeSmall).EncodeToBytes(new MozJpegEncoder(TargetJpegQuality, true)))
                    .EncodeToBytes(new MozJpegEncoder(TargetJpegQuality, true))
                    .Finish().InProcessAsync();

                imagesSet.SmallImage = jobResult.TryGet(1).TryGetBytes().Value.ToArray();
                imagesSet.LargeImage = jobResult.TryGet(2).TryGetBytes().Value.ToArray();

                return imagesSet;
            }
        }


        public async Task<Result> Remove(int accommodationId, Guid imageId)
        {
            return await _contractManagerContext.GetContractManager()
                .Tap(async contractManager => 
                { 
                    bool result = await RemoveImage(contractManager.Id, accommodationId, imageId);
                    return result ? Result.Success(contractManager) : Result.Failure<ContractManager>("Image deletion error");
                });


            async Task<bool> RemoveImage(int contractManagerId, int accommodationId, Guid imageId)
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
        }


        private Image Create(int contractManagerId, Models.Requests.Image image) => new Image
        {
            AccommodationId = image.AccommodationId,
            OriginalName = image.UploadedFile.FileName,
            OriginalContentType = image.UploadedFile.ContentType,
            LargeImageKey = string.Empty,
            SmallImageKey = string.Empty,
            ContractManagerId = contractManagerId
        };


        private Models.Responses.Image Build(Maybe<Image> image)
            => new Models.Responses.Image(image.Value.Id, image.Value.OriginalName, image.Value.OriginalContentType, image.Value.LargeImageKey, image.Value.SmallImageKey, image.Value.AccommodationId);


        private const string S3FolderName = "images";
        private const long MinimumImageWidth = 500;
        private const long MinimumImageHeight = 300;
        private const int MaximumSideSizeLarge = 1600;
        private const int MaximumSideSizeSmall = 400;
        private const int TargetJpegQuality = 85;


        private readonly IContractManagerContextService _contractManagerContext;
        private readonly DirectContractsDbContext _dbContext;
        private readonly IAmazonS3ClientService _amazonS3ClientService;
        private readonly string _bucketName;
    }
}
