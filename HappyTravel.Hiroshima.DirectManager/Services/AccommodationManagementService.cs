using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Geography;
using HappyTravel.Hiroshima.Common.Infrastructure.Extensions;
using HappyTravel.Hiroshima.Common.Infrastructure.Utilities;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Common.Models.Enums;
using HappyTravel.Hiroshima.DirectManager.RequestValidators;
using Accommodation = HappyTravel.Hiroshima.Data.Models.Accommodations.Accommodation;
using NetTopologySuite.Geometries;
using PropertyTypes = HappyTravel.Hiroshima.Common.Models.Accommodations.PropertyTypes;
using Room = HappyTravel.Hiroshima.Data.Models.Rooms.Room;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public class AccommodationManagementService : IAccommodationManagementService
    {
        public AccommodationManagementService(
            DirectContracts.Services.Management.IAccommodationManagementRepository accommodationManagementRepository,
            IContractManagerContextService contractManagerContext,
            GeometryFactory geometryFactory)
        {
            _accommodationManagementRepository = accommodationManagementRepository;
            _contractManagerContext = contractManagerContext;
            _geometryFactory = geometryFactory;
        }


        public Task<Result<Models.Responses.Accommodation>> GetAccommodation(int accommodationId)
        {
            return _contractManagerContext.GetContractManager()
                .Bind(async contractManager
                    =>
                {
                    var accommodation =
                        await _accommodationManagementRepository.GetAccommodation(contractManager.Id, accommodationId);
                    if (accommodation is null)
                        return Result.Failure<Models.Responses.Accommodation>(
                            $"Failed to get an accommodation by {nameof(accommodationId)} '{accommodationId}'");

                    var rooms = await _accommodationManagementRepository.GetRooms(accommodation.Id);

                    var roomIds = rooms is null
                        ? new List<int>()
                        : rooms.Select(room => room.Id).ToList();

                    return Result.Ok(CreateAccommodationResponse(accommodation, roomIds));
                });
        }


        public Task<Result<Models.Responses.Accommodation>> AddAccommodation(Models.Requests.Accommodation accommodation)
        {
            return Validate(accommodation)
                .Bind(() => _contractManagerContext.GetContractManager())
                .Bind(async contractManager =>
                {
                    var newAccommodation = await _accommodationManagementRepository.AddAccommodation(
                        CreateAccommodation(contractManager.Id, accommodation));
                    
                    return Result.Success(CreateAccommodationResponse(newAccommodation));
                });
        }
        
        
        public Task<Result> Update(int accommodationId, Models.Requests.Accommodation accommodation)
        {
            return _contractManagerContext.GetContractManager()
                .Bind(async contractManager =>
                {
                    await _accommodationManagementRepository.UpdateAccommodation(
                        CreateAccommodation(contractManager.Id, accommodation));
                    return Result.Success();

                });
        }

        
        public Task<Result> RemoveAccommodation(int accommodationId)
        {
            return  _contractManagerContext.GetContractManager()
                .Bind(async contractManager 
                    =>
                {
                    await _accommodationManagementRepository.DeleteAccommodationAndRooms(contractManager.Id,
                        accommodationId);
                    return Result.Success();
                });
        }


        private Result Validate(Models.Requests.Accommodation accommodation)
        {
            var validator = new AccommodationValidator();
            var validationResult = validator.Validate(accommodation);

            return validationResult.IsValid
                ? Result.Success()
                : Result.Combine(validationResult
                    .Errors
                    .Select(e => Result.Failure(e.ErrorMessage))
                    .ToArray());
        }


        private Accommodation CreateAccommodation(int contractManagerId, Models.Requests.Accommodation accommodation)
        {
            return new Accommodation
            {
                Name = JsonDocumentUtilities.CreateJDocument(accommodation.Name),
                Address = JsonDocumentUtilities.CreateJDocument(accommodation.Address),
                Coordinates = _geometryFactory.CreatePoint(new Coordinate(accommodation.Coordinates.Longitude, accommodation.Coordinates.Latitude)),
                Pictures = JsonDocumentUtilities.CreateJDocument(accommodation.Pictures),
                AccommodationAmenities = JsonDocumentUtilities.CreateJDocument(accommodation.Amenities),
                TextualDescription = JsonDocumentUtilities.CreateJDocument(accommodation.Description),
                Rating = accommodation.Rating,
                ContractManagerId = contractManagerId,
                ContactInfo = accommodation.ContactInfo,
                AdditionalInfo = JsonDocumentUtilities.CreateJDocument(accommodation.AdditionalInfo),
                OccupancyDefinition = accommodation.OccupancyDefinition,
                PropertyType = accommodation.Type,
                CheckInTime = accommodation.CheckInTime,
                CheckOutTime = accommodation.CheckOutTime,
                LocationId = accommodation.LocationId,
            };
        }

        
        private Models.Responses.Accommodation CreateAccommodationResponse(Accommodation accommodation, List<int> roomIds = null)
        {
            return new Models.Responses.Accommodation
            {
                Id = accommodation.Id,
                Coordinates = new GeoPoint(accommodation.Coordinates) ,
                Rating = accommodation.Rating ?? AccommodationRating.NotRated,
                CheckInTime = accommodation.CheckInTime,
                CheckOutTime = accommodation.CheckOutTime,
                ContactInfo = accommodation.ContactInfo,
                OccupancyDefinition = accommodation.OccupancyDefinition,
                PropertyType = accommodation.PropertyType ?? PropertyTypes.Any,
                Name = accommodation.Name.GetValue<MultiLanguage<string>>(),
                Address = accommodation.Address.GetValue<MultiLanguage<string>>(),
                Pictures = accommodation.Pictures.GetValue<MultiLanguage<List<Picture>>>(),
                Amenities = accommodation.AccommodationAmenities.GetValue<MultiLanguage<List<string>>>(),
                AdditionalInfo = accommodation.AdditionalInfo.GetValue<MultiLanguage<string>>(),
                TextualDescription = accommodation.TextualDescription.GetValue<MultiLanguage<string>>(),
                RoomIds = roomIds ?? new List<int>() 
            };
        }
        
            
        private async Task<List<Room>> CreateRooms(int accommodationId, List<Models.Requests.Room> rooms)
        {
            var newRooms = rooms.Select(room => new Room
            {
                AccommodationId = accommodationId,
                Name = JsonDocumentUtilities.CreateJDocument(room.Name),
                Description = JsonDocumentUtilities.CreateJDocument(room.Description),
                Amenities = JsonDocumentUtilities.CreateJDocument(room.Amenities),
                OccupancyConfigurations = room.OccupancyConfigurations,
                
            });
                
            return await _accommodationManagementRepository.AddRooms(newRooms.ToList());
        }

        
        private readonly IContractManagerContextService _contractManagerContext;
        private readonly GeometryFactory _geometryFactory;
        private readonly DirectContracts.Services.Management.IAccommodationManagementRepository
            _accommodationManagementRepository;
    }
}