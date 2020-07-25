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
        public AccommodationManagementService(DirectContracts.Services.Management.IAccommodationManagementRepository accommodationManagementRepository,
            IContractManagerContextService contractManagerContext, GeometryFactory geometryFactory)
        {
            _accommodationManagementRepository = accommodationManagementRepository;
            _contractManagerContext = contractManagerContext;
            _geometryFactory = geometryFactory;
        }


        public Task<Result<Models.Responses.Accommodation>> Get(int accommodationId)
        {
            return _contractManagerContext.GetContractManager()
                .Bind(async contractManager =>
                {
                    var accommodation = await _accommodationManagementRepository.GetAccommodation(contractManager.Id, accommodationId);
                    if (accommodation is null)
                        return Result.Failure<Models.Responses.Accommodation>(
                            $"Failed to get an accommodation by {nameof(accommodationId)} '{accommodationId}'");

                    var rooms = await _accommodationManagementRepository.GetRooms(accommodation.Id);

                    return Result.Ok(CreateResponse(accommodation, rooms.Select(room => room.Id).ToList()));
                });
        }


        public Task<Result<Models.Responses.Accommodation>> Add(Models.Requests.Accommodation accommodation)
        {
            return ValidateAccommodation(accommodation)
                .Bind(() => _contractManagerContext.GetContractManager())
                .Bind(async contractManager =>
                {
                    var newAccommodation = await _accommodationManagementRepository.AddAccommodation(CreateAccommodation(contractManager.Id, accommodation));

                    return Result.Success(CreateResponse(newAccommodation));
                });
        }


        public Task<Result> Update(int accommodationId, Models.Requests.Accommodation accommodation)
        {
            return _contractManagerContext.GetContractManager()
                .Bind(async contractManager =>
                {
                    await _accommodationManagementRepository.UpdateAccommodation(CreateAccommodation(contractManager.Id, accommodation));
                    return Result.Success();
                });
        }


        public Task<Result> Remove(int accommodationId)
        {
            return _contractManagerContext.GetContractManager()
                .Bind(async contractManager =>
                {
                    await _accommodationManagementRepository.DeleteAccommodationAndRooms(contractManager.Id, accommodationId);
                    return Result.Success();
                });
        }


        public Task<Result<List<Models.Responses.Room>>> GetRooms(int accommodationId)
        {
            return _contractManagerContext.GetContractManager()
                .Bind(async contractManager =>
                {
                    var rooms = await _accommodationManagementRepository.GetRooms(contractManager.Id, accommodationId);
                    return Result.Success(CreateResponse(rooms));
                });
        }


        public Task<Result<List<Models.Responses.Room>>> AddRooms(int accommodationId, List<Models.Requests.Room> rooms)
        {
            return ValidateRooms(rooms)
                .Bind(() => _contractManagerContext.GetContractManager())
                .Ensure(async contractManager => await _accommodationManagementRepository.GetAccommodation(contractManager.Id, accommodationId) != null,
                    $"Failed to get the accommodation by ID '{accommodationId}'")
                .Bind(async contractManager =>
                {
                    var newRooms = CreateRooms(accommodationId, rooms);
                    await _accommodationManagementRepository.AddRooms(newRooms);

                    return Result.Success(CreateResponse(newRooms));
                });
        }


        public Task<Result> RemoveRooms(int accommodationId, List<int> roomIds)
        {
            return _contractManagerContext.GetContractManager()
                .Bind(async contractManager =>
                {
                    var rooms = await _accommodationManagementRepository.GetRooms(contractManager.Id, accommodationId);
                    if (!rooms.Any())
                        return Result.Failure<List<int>>("IDs do not match rooms");

                    var ids = rooms.Select(r => r.Id).Intersect(roomIds).ToList();
                    return ids.Any() ? Result.Success(ids) : Result.Failure<List<int>>("IDs do not match rooms");
                })
                .Bind(async ids =>
                {
                    await _accommodationManagementRepository.DeleteRooms(ids);
                    return Result.Ok();
                });
        }


        private Result ValidateAccommodation(Models.Requests.Accommodation accommodation)
        {
            var validator = new AccommodationValidator();
            var validationResult = validator.Validate(accommodation);

            return validationResult.IsValid
                ? Result.Success()
                : Result.Combine(validationResult.Errors.Select(e => Result.Failure($"{e.PropertyName}: {e.ErrorMessage}")).ToArray());
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


        private Models.Responses.Accommodation CreateResponse(Accommodation accommodation, List<int> roomIds = null)
        {
            return new Models.Responses.Accommodation(
                accommodation.Id,
                coordinates: new GeoPoint(accommodation.Coordinates),
                rating: accommodation.Rating ?? AccommodationRating.NotRated, 
                checkInTime: accommodation.CheckInTime, 
                checkOutTime: accommodation.CheckOutTime,
                contactInfo: accommodation.ContactInfo, 
                occupancyDefinition: accommodation.OccupancyDefinition,
                propertyType: accommodation.PropertyType ?? PropertyTypes.Any, 
                name: accommodation.Name.GetValue<MultiLanguage<string>>(),
                address: accommodation.Address.GetValue<MultiLanguage<string>>(), 
                pictures: accommodation.Pictures.GetValue<MultiLanguage<List<Picture>>>(),
                amenities: accommodation.AccommodationAmenities.GetValue<MultiLanguage<List<string>>>(),
                additionalInfo: accommodation.AdditionalInfo.GetValue<MultiLanguage<string>>(),
                textualDescription: accommodation.TextualDescription.GetValue<MultiLanguage<string>>(),
                roomIds: roomIds ?? new List<int>());
        }


        private Result ValidateRooms(List<Models.Requests.Room> rooms)
        {
            var validator = new RoomValidator();
            List<string> errors = null;
            var validationFailure = false;
            foreach (var room in rooms)
            {
                var validationResult = validator.Validate(room);
                if (validationResult.IsValid)
                    continue;

                validationFailure = true;
                var errorMessages = validationResult.Errors.Select(e => $"{e.PropertyName}: " + e.ErrorMessage).Where(e => !string.IsNullOrEmpty(e));
                if (errors != null)
                    errors.AddRange(errorMessages);
                else
                    errors = new List<string>(errorMessages);
            }

            return !validationFailure ? Result.Success() : Result.Failure(string.Join("; ", errors));
        }


        private List<Room> CreateRooms(int accommodationId, List<Models.Requests.Room> rooms)
        {
            return rooms.Select(room => new Room
                {
                    AccommodationId = accommodationId,
                    Name = JsonDocumentUtilities.CreateJDocument(room.Name),
                    Description = JsonDocumentUtilities.CreateJDocument(room.Description),
                    Amenities = JsonDocumentUtilities.CreateJDocument(room.Amenities),
                    Pictures = JsonDocumentUtilities.CreateJDocument(room.Pictures),
                    OccupancyConfigurations = room.OccupancyConfigurations
                })
                .ToList();
        }


        private List<Models.Responses.Room> CreateResponse(List<Room> rooms)
        {
            return rooms.Select(room => new Models.Responses.Room(id: room.Id, name: room.Name.GetValue<MultiLanguage<string>>(),
                    description: room.Description.GetValue<MultiLanguage<string>>(), amenities: room.Amenities.GetValue<MultiLanguage<List<string>>>(),
                    pictures: room.Pictures.GetValue<MultiLanguage<List<Picture>>>(), occupancyConfigurations: room.OccupancyConfigurations))
                .ToList();
        }


        private readonly IContractManagerContextService _contractManagerContext;
        private readonly GeometryFactory _geometryFactory;
        private readonly DirectContracts.Services.Management.IAccommodationManagementRepository _accommodationManagementRepository;
    }
}