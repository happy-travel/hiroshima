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
using HappyTravel.Hiroshima.Common.Models.Images;
using HappyTravel.Hiroshima.Common.Models.Enums;

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
            var amazonS3RegionEndpoint = options.Value.AmazonS3RegionEndpoint;
            _basePathToAmazon = $"https://{_bucketName}.s3-{amazonS3RegionEndpoint}.amazonaws.com";
        }


        public Task<Result<List<Models.Responses.SlimImage>>> Get(int accommodationId)
        {
            return _contractManagerContext.GetContractManager()
                .EnsureAccommodationBelongsToContractManager(_dbContext, accommodationId)
                .Map(async contractManager =>
                {
                    return await _dbContext.Images
                        .Where(image => image.ContractManagerId == contractManager.Id && image.ReferenceId == accommodationId && image.ImageType == ImageTypes.AccommodationImage)
                        .OrderBy(image => image.Position)
                        .ToListAsync();
                })
                .Map(Build);
        }


        public Task<Result<List<Models.Responses.SlimImage>>> Get(int accommodationId, int roomId)
        {
            return _contractManagerContext.GetContractManager()
                .EnsureRoomBelongsToContractManager(_dbContext, accommodationId, roomId)
                .Map(async contractManager =>
                {
                    return await _dbContext.Images
                        .Where(image => image.ContractManagerId == contractManager.Id && image.ReferenceId == roomId && image.ImageType == ImageTypes.RoomImage)
                        .OrderBy(image => image.Position)
                        .ToListAsync();
                })
                .Map(Build);
        }


        public Task<Result<Guid>> Add(Models.Requests.Image image)
        {
            return _contractManagerContext.GetContractManager()
                .EnsureAccommodationBelongsToContractManager(_dbContext, image.AccommodationId)
                .Tap(contractManager => ValidationHelper.Validate(image, new ImageValidator()))
                .Tap(contractManager => ResortImages(contractManager.Id, image.AccommodationId))
                .Map(contractManager => Create(contractManager.Id, image))
                .Ensure(dbImage => ValidateImageType(image.UploadedFile).Value, "Invalid image file type")
                .Bind(dbImage => ConvertAndUpload(dbImage, image.UploadedFile));


            Task<Result<Guid>> ConvertAndUpload(Image dbImage, FormFile uploadedFile)
            {
                return GetBytes()
                    .Ensure(AreDimensionsValid,
                        $"Uploading image size must be at least {MinimumImageWidth}×{MinimumImageHeight} pixels and the width mustn't exceed two heights and vice versa")
                    .Bind(Convert)
                    .Bind(Upload);
                

                Result<byte[]> GetBytes()
                {
                    using var binaryReader = new BinaryReader(uploadedFile.OpenReadStream());
                    return Result.Success(binaryReader.ReadBytes((int)uploadedFile.Length));
                }
                

                async Task<bool> AreDimensionsValid(byte[] imageBytes)
                {
                    var info = await ImageJob.GetImageInfo(new BytesSource(imageBytes));

                    return MinimumImageWidth <= info.ImageWidth && 
                        MinimumImageHeight <= info.ImageHeight &&
                        info.ImageWidth / info.ImageHeight < 2 && 
                        info.ImageHeight / info.ImageWidth < 2;
                }

                async Task<Result<ImageSet>> Convert(byte[] imageBytes)
                {
                    var imagesSet = new ImageSet();

                    using var imageJob = new ImageJob();
                    var jobResult = await imageJob.Decode(imageBytes)
                        .Constrain(new Constraint(ConstraintMode.Within, ResizedLargeImageMaximumSideSize, ResizedLargeImageMaximumSideSize))
                        .Branch(f => f.ConstrainWithin(ResizedSmallImageMaximumSideSize, ResizedSmallImageMaximumSideSize).EncodeToBytes(new MozJpegEncoder(TargetJpegQuality, true)))
                        .EncodeToBytes(new MozJpegEncoder(TargetJpegQuality, true))
                        .Finish().InProcessAsync();

                    imagesSet.SmallImage = GetImage(1);
                    imagesSet.MainImage = GetImage(2);

                    return imagesSet.MainImage.Any() && imagesSet.SmallImage.Any()
                        ? Result.Success(imagesSet)
                        : Result.Failure<ImageSet>("Processing of the images failed");


                    byte[] GetImage(int index)
                    {
                        var encodeResult = jobResult?.TryGet(index);
                        var bytes = encodeResult?.TryGetBytes();
                        
                        return bytes != null ? bytes.Value.ToArray() : new byte[]{} ;
                    }
                }


                async Task<Result<Guid>> Upload(ImageSet imageSet)
                {
                    dbImage.Position = _dbContext.Images.Count(i => i.AccommodationId == dbImage.AccommodationId);
                    var entry = _dbContext.Images.Add(dbImage);
                    await _dbContext.SaveChangesAsync();
                    var imageId = entry.Entity.Id;

                    SetImageDetails();

                    var addToBucketResult = await AddImagesToBucket();
                    if (!addToBucketResult)
                    {
                        _dbContext.Images.Remove(entry.Entity);

                        await _dbContext.SaveChangesAsync();

                        return Result.Failure<Guid>("Uploading of the image failed");
                    }

                    _dbContext.Images.Update(entry.Entity);
                    
                    await _dbContext.SaveChangesAsync();

                    _dbContext.DetachEntry(entry.Entity);

                    return Result.Success(imageId);


                    void SetImageDetails()
                    {
                        var basePartOfKey = $"{S3FolderName}/{dbImage.AccommodationId}/{imageId}";
                        dbImage.MainImage.Key = $"{basePartOfKey}-main.jpg";
                        dbImage.SmallImage.Key = $"{basePartOfKey}-small.jpg";

                        dbImage.MainImage.Url = $"{_basePathToAmazon}/{dbImage.MainImage.Key}";
                        dbImage.SmallImage.Url = $"{_basePathToAmazon}/{dbImage.SmallImage.Key}";
                    }


                    async Task<bool> AddImagesToBucket()
                    {
                        await using var largeStream = new MemoryStream(imageSet.MainImage);
                        await using var smallStream = new MemoryStream(imageSet.SmallImage);
                        var imageList = new List<(string key, Stream stream)>
                        {
                            (dbImage.MainImage.Key, largeStream),
                            (dbImage.SmallImage.Key, smallStream)
                        };

                        var resultList = await _amazonS3ClientService.Add(_bucketName, imageList);
                        foreach (var result in resultList)
                        {
                            if (result.IsFailure)
                            {
                                var keyList = new List<string>
                                {
                                    dbImage.MainImage.Key,
                                    dbImage.SmallImage.Key,
                                };
                                await _amazonS3ClientService.Delete(_bucketName, keyList);

                                return false;
                            }
                        }

                        return true;
                    }
                }
            }
            
            Result<bool> ValidateImageType(FormFile uploadedFile)
            {
                var extension = Path.GetExtension(uploadedFile?.FileName)?.ToLower() ?? string.Empty;
                return extension == ".jpg" || extension == ".jpeg" || extension == ".png";
            }
        }


        public Task<Result> Update(int accommodationId, List<Models.Requests.SlimImage> images)
        {
            return _contractManagerContext.GetContractManager()
                .EnsureAccommodationBelongsToContractManager(_dbContext, accommodationId)
                .Map(async contractManager =>
                {
                    return await _dbContext.Images
                        .Where(image => image.ContractManagerId == contractManager.Id && image.ReferenceId == accommodationId && image.ImageType == ImageTypes.AccommodationImage)
                        .ToListAsync();
                })
                .Bind(dbImages => ArrangeImages(dbImages, images));
        }


        public Task<Result> Update(int accommodationId, int roomId, List<Models.Requests.SlimImage> images)
        {
            return _contractManagerContext.GetContractManager()
                .EnsureRoomBelongsToContractManager(_dbContext, accommodationId, roomId)
                .Map(async contractManager =>
                {
                    return await _dbContext.Images
                        .Where(image => image.ContractManagerId == contractManager.Id && image.ReferenceId == roomId && image.ImageType == ImageTypes.RoomImage)
                        .ToListAsync();
                })
                .Bind(dbImages => ArrangeImages(dbImages, images));
        }


        public async Task<Result> Remove(int accommodationId, Guid imageId)
        {
            return await _contractManagerContext.GetContractManager()
                .Tap(async contractManager => 
                { 
                    var result = await RemoveImage(contractManager.Id, accommodationId, ImageTypes.AccommodationImage, imageId);
                    return result ? Result.Success(contractManager) : Result.Failure<ContractManager>("Image deletion error");
                });
        }


        public async Task<Result> Remove(int accommodationId, int roomId, Guid imageId)
        {
            return await _contractManagerContext.GetContractManager()
                .Tap(async contractManager =>
                {
                    var result = await RemoveImage(contractManager.Id, roomId, ImageTypes.RoomImage, imageId);
                    return result ? Result.Success(contractManager) : Result.Failure<ContractManager>("Image deletion error");
                });
        }


        public async Task<Result> RemoveAll(int contractManagerId, int accommodationId)
        {
            var images = _dbContext.Images.Where(image => image.ContractManagerId == contractManagerId && 
                image.ReferenceId == accommodationId && image.ImageType == ImageTypes.AccommodationImage);
            
            foreach (var image in images)
            {
                var result = await RemoveImage(contractManagerId, accommodationId, image.ImageType, image.Id);
                if (!result)
                    return Result.Failure("Image deletion error");
            }
            return Result.Success();
        }


        public async Task<Result> RemoveAll(int contractManagerId, int accommodationId, int roomId)
        {
            var images = _dbContext.Images.Where(image => image.ContractManagerId == contractManagerId &&
                image.ReferenceId == roomId && image.ImageType == ImageTypes.RoomImage);
            
            foreach (var image in images)
            {
                var result = await RemoveImage(contractManagerId, roomId, image.ImageType, image.Id);
                if (!result)
                    return Result.Failure("Image deletion error");
            }
            return Result.Success();
        }


        private Image Create(int contractManagerId, Models.Requests.Image image) => new Image
        {
            AccommodationId = image.AccommodationId,
            OriginalImageDetails = new OriginalImageDetails
            {
                OriginalName = image.UploadedFile.FileName,
                OriginalContentType = image.UploadedFile.ContentType
            },
            ContractManagerId = contractManagerId,
            Created = DateTime.UtcNow,
            Description = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string> { Ar = string.Empty, En = string.Empty, Ru = string.Empty } )
        };


        private List<Models.Responses.SlimImage> Build(List<Image> images)
        {
            var slimImages = new List<Models.Responses.SlimImage>();
            foreach (var image in images)
            { 
                var slimImage = new Models.Responses.SlimImage
                    (
                        image.Id,
                        image.MainImage.Url,
                        image.SmallImage.Url,
                        image.Description.GetValue<MultiLanguage<string>>()
                    ); 
                slimImages.Add(slimImage);
            }
            return slimImages;
        }


        private async Task<Result> ArrangeImages(List<Image> dbImages, List<Models.Requests.SlimImage> images)
        {
            for (var i = 0; i < images.Count; i++)
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
        }


        private async Task<bool> RemoveImage(int contractManagerId, int referenceId, ImageTypes imageType, Guid imageId)
        {
            var image = await _dbContext.Images.SingleOrDefaultAsync(i => i.ContractManagerId == contractManagerId && 
                i.ReferenceId == referenceId && i.ImageType == imageType && i.Id == imageId);
            if (image is null)
                return false;

            var result = await _amazonS3ClientService.Delete(_bucketName, image.MainImage.Key);
            if (result.IsFailure)
                return false;

            result = await _amazonS3ClientService.Delete(_bucketName, image.SmallImage.Key);
            if (result.IsFailure)
                return false;

            _dbContext.Images.Remove(image);

            await _dbContext.SaveChangesAsync();

            return true;
        }


        private async Task ResortImages(int contractManagerId, int accommodationId)
        {
            var images = await _dbContext.Images
                .Where(image => image.ContractManagerId == contractManagerId && image.AccommodationId == accommodationId)
                .OrderBy(image => image.Position)
                .ToListAsync();

            for (var i = 0; i < images.Count; i++)
            {
                var image = images[i];
                image.Position = i;
                _dbContext.Images.Update(image);
            }
            await _dbContext.SaveChangesAsync();
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
        private readonly string _basePathToAmazon;
    }
}
