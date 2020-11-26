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
using System.Text.Json;

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


        public Task<Result<Guid>> Add(Models.Requests.AccommodationImage image)
        {
            return _contractManagerContext.GetContractManager()
                .EnsureAccommodationBelongsToContractManager(_dbContext, image.AccommodationId)
                .Bind(contractManager => Validate(image, contractManager))
                .Tap(contractManager => ResortImages(contractManager.Id, image.AccommodationId, ImageTypes.AccommodationImage))
                .Map(contractManager => Create(contractManager.Id, image))
                .Ensure(dbImage => ValidateImageType(image.UploadedFile).Value, "Invalid image file type")
                .Bind(dbImage => ConvertAndUpload(dbImage, image.UploadedFile))
                .Bind(AddSlimImageToAccommodation);

            Result<ContractManager> Validate(Models.Requests.AccommodationImage image, ContractManager contractManager)
            {
                var validationResult = ValidationHelper.Validate(image, new AccommodationImageValidator());
                return validationResult.IsFailure ? Result.Failure<ContractManager>(validationResult.Error) : Result.Success(contractManager);
            }
        }


        public Task<Result<Guid>> Add(Models.Requests.RoomImage image)
        {
            return _contractManagerContext.GetContractManager()
                .EnsureRoomBelongsToContractManager(_dbContext, image.AccommodationId, image.RoomId)
                .Bind(contractManager => Validate(image, contractManager))
                .Tap(contractManager => ResortImages(contractManager.Id, image.RoomId, ImageTypes.RoomImage))
                .Map(contractManager => Create(contractManager.Id, image))
                .Ensure(dbImage => ValidateImageType(image.UploadedFile).Value, "Invalid image file type")
                .Bind(dbImage => ConvertAndUpload(dbImage, image.UploadedFile))
                .Bind(AddSlimImageToRoom);

            Result<ContractManager> Validate(Models.Requests.RoomImage image, ContractManager contractManager)
            {
                var validationResult = ValidationHelper.Validate(image, new RoomImageValidator());
                return validationResult.IsFailure ? Result.Failure<ContractManager>(validationResult.Error) : Result.Success(contractManager);
            }
        }


        public Task<Result> Update(int accommodationId, List<Models.Requests.SlimImage> images)
        {
            return _contractManagerContext.GetContractManager()
                .EnsureAccommodationBelongsToContractManager(_dbContext, accommodationId)
                .Tap(async contractManager => await ArrangeSlimImagesInAccommodation(contractManager.Id, accommodationId, images))
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
                .Tap(async contractManager => await ArrangeSlimImagesInRoom(roomId, images))
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
                })
                .Tap(contractManager => RemoveSlimImageFromAccommodation(contractManager.Id, accommodationId, imageId));
        }


        public async Task<Result> Remove(int accommodationId, int roomId, Guid imageId)
        {
            return await _contractManagerContext.GetContractManager()
                .Tap(async contractManager =>
                {
                    var result = await RemoveImage(contractManager.Id, roomId, ImageTypes.RoomImage, imageId);
                    return result ? Result.Success(contractManager) : Result.Failure<ContractManager>("Image deletion error");
                })
                .Tap(contractManager => RemoveSlimImageFromRoom(roomId, imageId));
        }


        public async Task<Result> RemoveAll(int contractManagerId, int accommodationId)
        {
            var images = await _dbContext.Images
                .Where(image => image.ContractManagerId == contractManagerId && 
                    image.ReferenceId == accommodationId && image.ImageType == ImageTypes.AccommodationImage)
                .ToListAsync();
            
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
            var images = await _dbContext.Images
                .Where(image => image.ContractManagerId == contractManagerId && 
                    image.ReferenceId == roomId && image.ImageType == ImageTypes.RoomImage)
                .ToListAsync();
            
            foreach (var image in images)
            {
                var result = await RemoveImage(contractManagerId, roomId, image.ImageType, image.Id);
                if (!result)
                    return Result.Failure("Image deletion error");
            }
            return Result.Success();
        }


        public string GetImageUrl(string imageKey)
        {
            return $"{_basePathToAmazon}/{imageKey}";
        }


        private Image Create(int contractManagerId, Models.Requests.AccommodationImage image) => new Image
        {
            ReferenceId = image.AccommodationId,
            ImageType = ImageTypes.AccommodationImage,
            OriginalImageDetails = new OriginalImageDetails
            {
                OriginalName = image.UploadedFile.FileName,
                OriginalContentType = image.UploadedFile.ContentType
            },
            ContractManagerId = contractManagerId,
            Created = DateTime.UtcNow,
            Description = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string> { Ar = string.Empty, En = string.Empty, Ru = string.Empty } )
        };


        private Image Create(int contractManagerId, Models.Requests.RoomImage image) => new Image
        {
            ReferenceId = image.RoomId,
            ImageType = ImageTypes.RoomImage,
            OriginalImageDetails = new OriginalImageDetails
            {
                OriginalName = image.UploadedFile.FileName,
                OriginalContentType = image.UploadedFile.ContentType
            },
            ContractManagerId = contractManagerId,
            Created = DateTime.UtcNow,
            Description = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string> { Ar = string.Empty, En = string.Empty, Ru = string.Empty })
        };


        private Result<bool> ValidateImageType(FormFile uploadedFile)
        {
            var extension = Path.GetExtension(uploadedFile?.FileName)?.ToLower() ?? string.Empty;
            return extension == ".jpg" || extension == ".jpeg" || extension == ".png";
        }


        private Task<Result<Image>> ConvertAndUpload(Image dbImage, FormFile uploadedFile)
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

                    return bytes != null ? bytes.Value.ToArray() : new byte[] { };
                }
            }


            async Task<Result<Image>> Upload(ImageSet imageSet)
            {
                dbImage.Position = _dbContext.Images.Count(i => i.ReferenceId == dbImage.ReferenceId && i.ImageType == dbImage.ImageType);
                var entry = _dbContext.Images.Add(dbImage);
                await _dbContext.SaveChangesAsync();
                var imageId = entry.Entity.Id;

                SetImageKeys();

                var addToBucketResult = await AddImagesToBucket();
                if (!addToBucketResult)
                {
                    _dbContext.Images.Remove(entry.Entity);

                    await _dbContext.SaveChangesAsync();

                    return Result.Failure<Image>("Uploading of the image failed");
                }

                _dbContext.Images.Update(entry.Entity);

                await _dbContext.SaveChangesAsync();

                _dbContext.DetachEntry(entry.Entity);

                return Result.Success(dbImage);


                void SetImageKeys()
                {
                    var basePartOfKey = $"{S3FolderName}/{dbImage.ContractManagerId}/{imageId}";
                    dbImage.Keys.MainImage = $"{basePartOfKey}-main.jpg";
                    dbImage.Keys.SmallImage = $"{basePartOfKey}-small.jpg";
                }


                async Task<bool> AddImagesToBucket()
                {
                    await using var largeStream = new MemoryStream(imageSet.MainImage);
                    await using var smallStream = new MemoryStream(imageSet.SmallImage);
                    var imageList = new List<(string key, Stream stream)>
                        {
                            (dbImage.Keys.MainImage, largeStream),
                            (dbImage.Keys.SmallImage, smallStream)
                        };

                    var resultList = await _amazonS3ClientService.Add(_bucketName, imageList);
                    foreach (var result in resultList)
                    {
                        if (result.IsFailure)
                        {
                            var keyList = new List<string>
                                {
                                    dbImage.Keys.MainImage,
                                    dbImage.Keys.SmallImage
                                };
                            await _amazonS3ClientService.Delete(_bucketName, keyList);

                            return false;
                        }
                    }

                    return true;
                }
            }
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

            var result = await _amazonS3ClientService.Delete(_bucketName, image.Keys.MainImage);
            if (result.IsFailure)
                return false;

            result = await _amazonS3ClientService.Delete(_bucketName, image.Keys.SmallImage);
            if (result.IsFailure)
                return false;

            _dbContext.Images.Remove(image);

            await _dbContext.SaveChangesAsync();

            return true;
        }


        private async Task ResortImages(int contractManagerId, int referenceId, ImageTypes imageType)
        {
            var images = await _dbContext.Images
                .Where(image => image.ContractManagerId == contractManagerId && image.ReferenceId == referenceId && image.ImageType == imageType)
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


        private async Task<Result<Guid>> AddSlimImageToAccommodation(Image image)
        {
            var accommodation = _dbContext.Accommodations.SingleOrDefault(a => a.ContractManagerId == image.ContractManagerId && a.Id == image.ReferenceId);

            var slimImage = Build(image);
            accommodation.Images.Add(slimImage);

            _dbContext.Accommodations.Update(accommodation);
            await _dbContext.SaveChangesAsync();

            return Result.Success(image.Id);
        }


        private async Task<Result<Guid>> AddSlimImageToRoom(Image image)
        {
            var room = _dbContext.Rooms.SingleOrDefault(r => r.Id == image.ReferenceId);

            var slimImage = Build(image);
            room.Images.Add(slimImage);

            _dbContext.Rooms.Update(room);
            await _dbContext.SaveChangesAsync();

            return Result.Success(image.Id);
        }


        private async Task<Result> ArrangeSlimImagesInAccommodation(int contractManagerId, int accommodationId, List<Models.Requests.SlimImage> images)
        {
            var accommodation = _dbContext.Accommodations.SingleOrDefault(a => a.ContractManagerId == contractManagerId && a.Id == accommodationId);

            accommodation.Images = Build(images);

            _dbContext.Accommodations.Update(accommodation);
            await _dbContext.SaveChangesAsync();

            return Result.Success();
        }


        private async Task<Result> ArrangeSlimImagesInRoom(int roomId, List<Models.Requests.SlimImage> images)
        {
            var room = _dbContext.Rooms.SingleOrDefault(r => r.Id == roomId);

            room.Images = Build(images);

            _dbContext.Rooms.Update(room);
            await _dbContext.SaveChangesAsync();

            return Result.Success();
        }


        private async Task<Result> RemoveSlimImageFromAccommodation(int contractManagerId, int accommodationId, Guid imageId)
        {
            var accommodation = _dbContext.Accommodations.SingleOrDefault(a => a.ContractManagerId == contractManagerId && a.Id == accommodationId);

            var slimImage = accommodation.Images.SingleOrDefault(i => i.Id == imageId);
            if (slimImage == null)
                return Result.Failure("Image not found in accommodation");

            accommodation.Images.Remove(slimImage);

            _dbContext.Accommodations.Update(accommodation);
            await _dbContext.SaveChangesAsync();

            return Result.Success();
        }


        private async Task<Result> RemoveSlimImageFromRoom(int roomId, Guid imageId)
        {
            var room = _dbContext.Rooms.SingleOrDefault(r => r.Id == roomId);

            var slimImage = room.Images.SingleOrDefault(i => i.Id == imageId);
            if (slimImage == null)
                return Result.Failure("Image not found in accommodation");

            room.Images.Remove(slimImage);

            _dbContext.Rooms.Update(room);
            await _dbContext.SaveChangesAsync();

            return Result.Success();
        }


        private SlimImage Build(Image image)
        {
            return new SlimImage
            {
                Id = image.Id,
                LargeImageURL = GetImageUrl(image.Keys.MainImage),
                SmallImageURL = GetImageUrl(image.Keys.SmallImage),
                Description = image.Description.GetValue<MultiLanguage<string>>()
            };
        }


        private List<SlimImage> Build(List<Models.Requests.SlimImage> images)
        {
            var slimImages = new List<SlimImage>(images.Count);
            foreach (var image in images)
            {
                var slimImage = new SlimImage
                {
                    Id = image.Id,
                    LargeImageURL = image.LargeImageURL,
                    SmallImageURL = image.SmallImageURL,
                    Description = image.Description
                };
                slimImages.Add(slimImage);
            }
            return slimImages;
        }


        private List<Models.Responses.SlimImage> Build(List<Image> images)
        {
            var slimImages = new List<Models.Responses.SlimImage>(images.Count);
            foreach (var image in images)
            {
                var slimImage = new Models.Responses.SlimImage
                    (
                        image.Id,
                        GetImageUrl(image.Keys.MainImage),
                        GetImageUrl(image.Keys.SmallImage),
                        image.Description.GetValue<MultiLanguage<string>>()
                    );
                slimImages.Add(slimImage);
            }
            return slimImages;
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
