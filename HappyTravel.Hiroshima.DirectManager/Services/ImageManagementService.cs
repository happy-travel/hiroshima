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
                //.Map(dbImage => ValidateImage(image.UploadedFile))
                .Map(dbImage => AddImage(dbImage, image.UploadedFile))
                .Ensure(dbImage => dbImage != null, $"Error saving image")
                .Map(dbImage => Build(dbImage));


            async Task<Image> AddImage(Image dbImage, FormFile uploadedFile)
            {
                var imageBytes = await ValidateImage(uploadedFile);
                
                var imageSet = await ConvertImage(imageBytes.Value);

                var extension = Path.GetExtension(uploadedFile.FileName);
                dbImage.Created = DateTime.UtcNow;

                var entry = _dbContext.Images.Add(dbImage);

                await _dbContext.SaveChangesAsync();

                entry.Entity.LargeImageKey = $"{S3FolderName}/{dbImage.AccommodationId}/{entry.Entity.Id}-large.jpg";

                var stream = new MemoryStream(imageSet.LargeImage);
                var result = await _amazonS3ClientService.Add(_bucketName, dbImage.LargeImageKey, stream);
                if (result.IsFailure)
                {
                    _dbContext.Images.Remove(entry.Entity);

                    await _dbContext.SaveChangesAsync();

                    return null;
                }

                entry.Entity.SmallImageKey = $"{S3FolderName}/{dbImage.AccommodationId}/{entry.Entity.Id}-small.jpg";

                stream = new MemoryStream(imageSet.SmallImage);
                result = await _amazonS3ClientService.Add(_bucketName, dbImage.SmallImageKey, stream);
                if (result.IsFailure)
                {
                    _dbContext.Images.Remove(entry.Entity);

                    await _dbContext.SaveChangesAsync();

                    return null;
                }

                await _dbContext.SaveChangesAsync();

                _dbContext.DetachEntry(entry.Entity);

                return entry.Entity;
            }

            async Task<Result<byte[]>> ValidateImage(FormFile uploadedFile)
            {
                // Validation image type
                var extension = Path.GetExtension(uploadedFile.FileName).ToLower();
                if ((extension != ".jpg") && (extension != ".jpeg") && (extension != ".png"))
                    return Result.Failure<byte[]>("Invalid image file type");

                byte[] imageBytes = null;
                using (BinaryReader binaryReader = new BinaryReader(uploadedFile.OpenReadStream()))
                {
                    imageBytes = binaryReader.ReadBytes((int)uploadedFile.Length);
                }

                var info = await ImageJob.GetImageInfo(new BytesSource(imageBytes));

                // Validation image size
                if ((info.ImageWidth < minimumImageWidth) || (info.ImageHeight < minimumImageHeight))
                    return Result.Failure<byte[]>("Uploading picture size is less than the minimum");

                // Validation image dimension difference
                if ((info.ImageWidth / info.ImageHeight > 2) || (info.ImageHeight / info.ImageWidth > 2))
                    return Result.Failure<byte[]>("Uploading picture dimension difference is more then the maximum");

                return imageBytes;
            }

            async Task<ImagesSet> ConvertImage(byte[] imageBytes)
            {
                ImagesSet imagesSet = new ImagesSet();

                using (var imageJob = new ImageJob())
                {
                    var jobResult = await imageJob.Decode(imageBytes).
                        Constrain(new Constraint(ConstraintMode.Within, maximumSideSizeLarge, maximumSideSizeLarge))
                        .Branch(f => f.ConstrainWithin(maximumSideSizeSmall, maximumSideSizeSmall).EncodeToBytes(new MozJpegEncoder(targetJpegQuality, true)))
                        .EncodeToBytes(new MozJpegEncoder(targetJpegQuality, true))
                        .Finish().InProcessAsync();

                    imagesSet.SmallImage = jobResult.TryGet(1).TryGetBytes().Value.ToArray();
                    imagesSet.LargeImage = jobResult.TryGet(2).TryGetBytes().Value.ToArray();
                }

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
            LargeImageKey = "",
            SmallImageKey = "",
            ContractManagerId = contractManagerId
        };


        private Models.Responses.Image Build(Image image)
            => new Models.Responses.Image(image.Id, image.OriginalName, image.OriginalContentType, image.LargeImageKey, image.SmallImageKey, image.AccommodationId);


        private const string S3FolderName = "images";
        private const long minimumImageWidth = 500;
        private const long minimumImageHeight = 300;
        private const int maximumSideSizeLarge = 1600;
        private const int maximumSideSizeSmall = 400;
        private const int targetJpegQuality = 85;


        private readonly IContractManagerContextService _contractManagerContext;
        private readonly DirectContractsDbContext _dbContext;
        private readonly IAmazonS3ClientService _amazonS3ClientService;
        private readonly string _bucketName;
    }
}
