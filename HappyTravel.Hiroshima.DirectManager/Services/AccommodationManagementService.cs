using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Geography;
using HappyTravel.Hiroshima.Common.Infrastructure.Extensions;
using HappyTravel.Hiroshima.Common.Infrastructure.Utilities;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Common.Models.Accommodations;
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.Data.Extensions;
using HappyTravel.Hiroshima.Data.Models;
using HappyTravel.Hiroshima.DirectManager.Infrastructure;
using HappyTravel.Hiroshima.DirectManager.RequestValidators;
using Microsoft.EntityFrameworkCore;
using Accommodation = HappyTravel.Hiroshima.Data.Models.Accommodations.Accommodation;
using NetTopologySuite.Geometries;
using Room = HappyTravel.Hiroshima.Data.Models.Rooms.Room;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public class AccommodationManagementService : IAccommodationManagementService
    {
        public AccommodationManagementService(DirectContracts.Services.Management.IAccommodationManagementRepository accommodationManagementRepository,
            IContractManagerContextService contractManagerContext, DirectContractsDbContext dbContext, GeometryFactory geometryFactory)
        {
            _accommodationManagementRepository = accommodationManagementRepository;
            _contractManagerContext = contractManagerContext;
            _geometryFactory = geometryFactory;
            _dbContext = dbContext;
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

                    return Create(accommodation, rooms.Select(room => room.Id).ToList());
                });
        }

        
        public Task<Result<List<Models.Responses.Accommodation>>> Get()
        {
            return _contractManagerContext.GetContractManager()
                .Map(contractManager => GetContractManagerAccommodationsWithRoomIds(contractManager.Id))
                .Map(accommodationWithRoomIds =>
                    accommodationWithRoomIds.Select(accommodation
                        => Create(accommodation.accommodation, accommodation.roomIds)
                    ).ToList()
                );
        }


        private async Task<List<(Accommodation accommodation, List<int> roomIds)>> GetContractManagerAccommodationsWithRoomIds(int contractManagerId)
        {
            var accommodations = await _dbContext.Accommodations
                .Where(accommodation => accommodation.ContractManagerId == contractManagerId)
                .ToListAsync();

            var accommodationWithRoomIds = new List<(Accommodation accommodation, List<int> roomIds)>(accommodations.Count);
            foreach (var accommodation in accommodations)
            {
                var accommodationRooms = await _dbContext.Rooms.Where(room => room.AccommodationId == accommodation.Id).Select(room => room.Id).ToListAsync();
                
                accommodationWithRoomIds.Add((accommodation, accommodationRooms));
            }
            
            return accommodationWithRoomIds;
        }


        public Task<Result<Models.Responses.Accommodation>> Add(Models.Requests.Accommodation accommodation)
        {
            return ValidateAccommodation(accommodation)
                .Bind(() => _contractManagerContext.GetContractManager())
                .Map(async contractManager =>
                {
                    var entry = _dbContext.Accommodations.Add(CreateAccommodation(contractManager.Id, accommodation));
                    await _dbContext.SaveChangesAsync();
                    entry.State = EntityState.Detached;
                    return Create(entry.Entity);
                });
        }


        public Task<Result<Models.Responses.Accommodation>> Update(int accommodationId, Models.Requests.Accommodation accommodation)
        {
            return ValidateAccommodation(accommodation)
                .Bind(() => _contractManagerContext.GetContractManager())
                .Ensure(contractManager => DoesAccommodationBelongToContractManager(accommodationId, contractManager.Id), 
                    $"Failed to get an accommodation by {nameof(accommodationId)} '{accommodationId}'")
                .Map(async contractManager =>
                {
                    var dbAccommodation = CreateAccommodation(contractManager.Id, accommodation);
                    dbAccommodation.Id = accommodationId;
                    var entry = _dbContext.Accommodations.Update(dbAccommodation);
                    await _dbContext.SaveChangesAsync();
                 
                    return Create(entry.Entity);
                });
        }


        public Task<Result> Remove(int accommodationId)
        {
            return _contractManagerContext.GetContractManager()
                .Bind(RemoveAccommodationWithRooms);
            
            async Task<Result> RemoveAccommodationWithRooms(ContractManager contractManager)
            {
                var accommodation = await _accommodationManagementRepository.GetAccommodation(contractManager.Id, accommodationId);
                    
                if (accommodation is null)
                    return Result.Success();
            
                var rooms = await _accommodationManagementRepository.GetRooms(accommodation.Id);
                if (rooms.Any())
                    await _accommodationManagementRepository.DeleteRooms(rooms.Select(r => r.Id).ToList());

                _dbContext.Accommodations.Remove(new Accommodation
                {
                    Id = accommodationId
                });
                await _dbContext.SaveChangesAsync();
                    
                return Result.Success();
            }
        }


        public Task<Result<List<Models.Responses.Room>>> GetRooms(int accommodationId)
        {
            return _contractManagerContext.GetContractManager()
                .Bind(async contractManager =>
                {
                    var rooms = await _accommodationManagementRepository.GetRooms(contractManager.Id, accommodationId);
                    
                    return Create(rooms);
                });
        }


        public Task<Result<Models.Responses.Room>> UpdateRoom(int accommodationId, int roomId, Models.Requests.Room room)
        {
            return ValidateRoom()
                .Bind(() => _contractManagerContext.GetContractManager())
                .Ensure(contractManager => DoesAccommodationBelongToContractManager(accommodationId, contractManager.Id), $"The room with {nameof(roomId)} '{roomId}' doesn't belong to the contract manager")
                .Ensure(contractManager => DoesRoomBelongToAccommodation(accommodationId, roomId), $"The room with {nameof(roomId)} '{roomId}' doesn't belong to the accommodation with {nameof(accommodationId)} '{accommodationId}'")
                .Map(manager => UpdateRoom());
                
            
                Result ValidateRoom()
                => GenericValidator<Models.Requests.Room>.Validate(configureAction => configureAction.RuleFor(exp => exp)
                    .SetValidator(new RoomValidator()), room);


                async Task<Models.Responses.Room> UpdateRoom()
                {
                    var dbRoom = CreateRoom(accommodationId, room);
                    dbRoom.Id = roomId;
                    _dbContext.Rooms.Update(dbRoom);
                    await _dbContext.SaveChangesAsync();
                    
                    return Create(dbRoom);
                }
        }
        
        

        public Task<Result<List<Models.Responses.Room>>> AddRooms(int accommodationId, List<Models.Requests.Room> rooms)
        {
            return ValidationHelper.Validate(rooms, new RoomValidator())
                .Bind(() => _contractManagerContext.GetContractManager())
                .Ensure(contractManager => DoesAccommodationBelongToContractManager(accommodationId, contractManager.Id),
                    $"Failed to get the accommodation by ID '{accommodationId}'")
                .Bind(async contractManager =>
                {
                    var newRooms = CreateRooms(accommodationId, rooms); 
                    _dbContext.Rooms.AddRange(newRooms);
                    await _dbContext.SaveChangesAsync();
                    _dbContext.DetachEntries(newRooms);

                    return Create(newRooms);
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
                    return Result.Success();
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
                ContactInfo =  new ContactInfo
                {
                    Email = accommodation.ContactInfo.Email,
                    Phone = accommodation.ContactInfo.Phone,
                    Website = accommodation.ContactInfo.Website
                },
                AdditionalInfo = JsonDocumentUtilities.CreateJDocument(accommodation.AdditionalInfo),
                OccupancyDefinition = accommodation.OccupancyDefinition,
                PropertyType = accommodation.Type,
                CheckInTime = accommodation.CheckInTime,
                CheckOutTime = accommodation.CheckOutTime,
                LocationId = accommodation.LocationId,
            };
        }


        private Models.Responses.Accommodation Create(Accommodation accommodation, List<int> roomIds = null)
        {
            return new Models.Responses.Accommodation(
                accommodation.Id,
                coordinates: new GeoPoint(accommodation.Coordinates),
                rating: accommodation.Rating,
                checkInTime: accommodation.CheckInTime, 
                checkOutTime: accommodation.CheckOutTime,
                contactInfo: accommodation.ContactInfo, 
                occupancyDefinition: accommodation.OccupancyDefinition,
                propertyType: accommodation.PropertyType, 
                name: accommodation.Name.GetValue<MultiLanguage<string>>(),
                address: accommodation.Address.GetValue<MultiLanguage<string>>(), 
                pictures: accommodation.Pictures.GetValue<MultiLanguage<List<Picture>>>(),
                amenities: accommodation.AccommodationAmenities.GetValue<MultiLanguage<List<string>>>(),
                additionalInfo: accommodation.AdditionalInfo.GetValue<MultiLanguage<string>>(),
                description: accommodation.TextualDescription.GetValue<MultiLanguage<TextualDescription>>(),
                locationId:accommodation.LocationId,
                roomIds: roomIds ?? new List<int>());
        }

        
        private List<Room> CreateRooms(int accommodationId, List<Models.Requests.Room> rooms) 
            => rooms.Select(room => CreateRoom(accommodationId, room))
            .ToList();


        private Room CreateRoom(int accommodationId, Models.Requests.Room room) => new Room
        {
            AccommodationId = accommodationId,
            Name = JsonDocumentUtilities.CreateJDocument(room.Name),
            Description = JsonDocumentUtilities.CreateJDocument(room.Description),
            Amenities = JsonDocumentUtilities.CreateJDocument(room.Amenities),
            Pictures = JsonDocumentUtilities.CreateJDocument(room.Pictures),  
            OccupancyConfigurations = room.OccupancyConfigurations
        };


        private static Result<List<Models.Responses.Room>> Create(List<Room> rooms) => rooms.Select(Create)
            .ToList();


        private static Models.Responses.Room Create(Room room)
            => new Models.Responses.Room(room.Id, room.Name.GetValue<MultiLanguage<string>>(), room.Description.GetValue<MultiLanguage<string>>(),
                room.Amenities.GetValue<MultiLanguage<List<string>>>(), room.Pictures.GetValue<MultiLanguage<List<Picture>>>(), room.OccupancyConfigurations);
        
        
        private async Task<bool> DoesRoomBelongToAccommodation(int accommodationId, int roomId) 
            => await _dbContext.Rooms.Where(room => room.AccommodationId == accommodationId && room.Id == roomId).SingleOrDefaultAsync() != null;

        
        private async Task<bool> DoesAccommodationBelongToContractManager(int accommodationId, int contractManagerId ) 
            => await _accommodationManagementRepository.GetAccommodation(contractManagerId, accommodationId) != null;


        private readonly IContractManagerContextService _contractManagerContext;
        private readonly GeometryFactory _geometryFactory;
        private readonly DirectContractsDbContext _dbContext;
        private readonly DirectContracts.Services.Management.IAccommodationManagementRepository _accommodationManagementRepository;
    }
}