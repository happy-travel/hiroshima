using CSharpFunctionalExtensions;
using HappyTravel.AmazonS3Client.Services;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.Data.Extensions;
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

            async Task<Result> ValidateImage(FormFile uploadedFile)
            {
                // Validation image type
                var extension = Path.GetExtension(uploadedFile.FileName).ToLower();
                if ((extension != ".ipg") && (extension != ".ipeg") && (extension != ".png"))
                    return Result.Failure("Invalid image file type");

                byte[] imageBytes = null;
                using (BinaryReader binaryReader = new BinaryReader(uploadedFile.OpenReadStream()))
                {
                    imageBytes = binaryReader.ReadBytes((int)imageBytes.Length);
                }

                var info = await ImageJob.GetImageInfo(new BytesSource(imageBytes));

                // Validation image size
                if ((info.ImageWidth < minimumImageWidth) || (info.ImageHeight < minimumImageHeight))
                    return Result.Failure("Uploading picture size is less than the minimum");

                // Validation image dimension difference
                if ((info.ImageWidth / info.ImageHeight > 2) || (info.ImageHeight / info.ImageWidth > 2))
                    return Result.Failure("Uploading picture dimension difference is more then the maximum");


                using (var b = new ImageJob())
                {
                    
                    var r = await b.Decode(imageBytes).
                        Constrain(new Constraint(ConstraintMode.Fit, 160, 120))
                        .Branch(f => f.ConstrainWithin(80, 60).EncodeToBytes(new WebPLosslessEncoder()))
                        .Branch(f => f.ConstrainWithin(40, 30).EncodeToBytes(new WebPLossyEncoder(85)))
                        .EncodeToBytes(new MozJpegEncoder(targetJpegQuality, true))
                        .Finish().InProcessAsync();

                    //Assert.Equal(60, r.TryGet(1).Width);
                    //Assert.Equal(30, r.TryGet(2).Width);
                    //Assert.Equal(120, r.TryGet(3).Width);
                    //Assert.True(r.First.TryGetBytes().HasValue);
                    var i = 0;
                }


                return Result.Success();
            }

            async Task<Image> AddImage(Image dbImage, FormFile uploadedFile)
            {
                var extension = Path.GetExtension(uploadedFile.FileName);
                dbImage.Id = Guid.NewGuid();
                dbImage.Key = $"{S3FolderName}/{dbImage.AccommodationId}/{dbImage.Id}{extension}";
                dbImage.Created = DateTime.UtcNow;

                await ValidateImage(uploadedFile);

                // Add document to Amazon S3
                var result = await _amazonS3ClientService.Add(_bucketName, dbImage.Key, uploadedFile.OpenReadStream());
                if (result.IsFailure)
                    return null;

                var entry = _dbContext.Images.Add(dbImage);

                await _dbContext.SaveChangesAsync();

                _dbContext.DetachEntry(entry.Entity);

                return entry.Entity;
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

                // Remove file from Amazon S3
                var result = await _amazonS3ClientService.Delete(_bucketName, image.Key);
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
            Name = image.UploadedFile.FileName,
            MimeType = image.UploadedFile.ContentType,
            ContractManagerId = contractManagerId
        };


        private Models.Responses.Image Build(Image image)
            => new Models.Responses.Image(image.Id, image.Name, image.Key, image.MimeType, image.AccommodationId);


        private const string S3FolderName = "images";
        private const long minimumImageWidth = 500;
        private const long minimumImageHeight = 300;
        private const long maximumSideSizeLarge = 1600;
        private const long maximumSideSizeSmall = 400;
        private const int targetJpegQuality = 85;


        private readonly IContractManagerContextService _contractManagerContext;
        private readonly DirectContractsDbContext _dbContext;
        private readonly IAmazonS3ClientService _amazonS3ClientService;
        private readonly string _bucketName;
    }
}
