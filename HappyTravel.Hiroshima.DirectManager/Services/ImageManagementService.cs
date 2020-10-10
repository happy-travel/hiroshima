using CSharpFunctionalExtensions;
using HappyTravel.AmazonS3Client.Services;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.Data.Extensions;
using HappyTravel.Hiroshima.DirectManager.RequestValidators;
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
            DirectContracts.Services.Management.IContractManagementRepository contractManagementRepository,
            DirectContractsDbContext dbContext, IAmazonS3ClientService amazonS3ClientService, IOptions<ImageManagementServiceOptions> options)
        {
            _contractManagerContext = contractManagerContextService;
            _contractManagementRepository = contractManagementRepository;
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
                .Map(dbImage => AddImage(dbImage, image.UploadedFile))
                .Ensure(dbImage => dbImage != null, $"Error saving image to Amazon S3")
                .Map(dbImage => Build(dbImage));


            async Task<Image> AddImage(Image dbImage, FormFile uploadedFile)
            {
                string extension = Path.GetExtension(uploadedFile.FileName);
                dbImage.Id = Guid.NewGuid();
                dbImage.Key = $"{S3FolderName}/{dbImage.AccommodationId}/{dbImage.Id}{extension}";
                dbImage.Created = DateTime.UtcNow;

                // Add document to Amazon S3
                var result = await _amazonS3ClientService.Add(_bucketName, dbImage.Key, uploadedFile.OpenReadStream());
                if (result.IsSuccess)
                {
                    var entry = _dbContext.Images.Add(dbImage);

                    await _dbContext.SaveChangesAsync();

                    _dbContext.DetachEntry(entry.Entity);

                    return entry.Entity;
                }
                else
                    return null;
            }
        }


        public async Task<Result> Remove(int accommodationId, Guid imageId)
        {
            return await _contractManagerContext.GetContractManager()
                .Tap(async contractManager => await RemoveImage(contractManager.Id, accommodationId, imageId));


            async Task RemoveImage(int contractManagerId, int accommodationId, Guid imageId)
            {
                var image = await _dbContext.Images.SingleOrDefaultAsync(c => c.ContractManagerId == contractManagerId && c.AccommodationId == accommodationId && c.Id == imageId);
                if (image is null)
                    return;

                // Remove file from Amazon S3
                var result = await _amazonS3ClientService.Delete(_bucketName, image.Key);
                if (result.IsSuccess)
                {
                    _dbContext.Images.Remove(image);

                    await _dbContext.SaveChangesAsync();
                }
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


        private readonly IContractManagerContextService _contractManagerContext;
        private readonly DirectContracts.Services.Management.IContractManagementRepository _contractManagementRepository;
        private readonly DirectContractsDbContext _dbContext;
        private readonly IAmazonS3ClientService _amazonS3ClientService;
        private readonly string _bucketName;
    }
}
