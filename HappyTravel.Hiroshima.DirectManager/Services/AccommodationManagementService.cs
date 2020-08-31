﻿using System.Collections.Generic;
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
using HappyTravel.Hiroshima.DirectManager.Infrastructure.Extensions;
using HappyTravel.Hiroshima.DirectManager.RequestValidators;
using Microsoft.EntityFrameworkCore;
using Accommodation = HappyTravel.Hiroshima.Data.Models.Accommodations.Accommodation;
using NetTopologySuite.Geometries;
using Room = HappyTravel.Hiroshima.Data.Models.Rooms.Room;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public class AccommodationManagementService : IAccommodationManagementService
    {
        public AccommodationManagementService(
            IContractManagerContextService contractManagerContext, DirectContractsDbContext dbContext, GeometryFactory geometryFactory)
        {
            _contractManagerContext = contractManagerContext;
            _geometryFactory = geometryFactory;
            _dbContext = dbContext;
        }


        public Task<Result<Models.Responses.Accommodation>> Get(int accommodationId)
        {
            return _contractManagerContext.GetContractManager()
                .Bind(async contractManager =>
                {
                    var accommodation = await GetAccommodationWithRooms(contractManager.Id, accommodationId).SingleOrDefaultAsync();
                    if (accommodation == null)
                        return Result.Failure<Models.Responses.Accommodation>(
                            $"Failed to get an accommodation by {nameof(accommodationId)} '{accommodationId}'");
                  
                    return Build(accommodation);
                });
        }

        
        public Task<Result<List<Models.Responses.Accommodation>>> Get(int skip, int top)
        {
            return _contractManagerContext.GetContractManager()
                .Map(contractManager => GetContractManagerAccommodationsWithRoomIds(contractManager.Id))
                .Map(accommodationWithRoomIds =>
                    accommodationWithRoomIds.Select(Build).ToList()
                );
            
            
            async Task<List<Accommodation>> GetContractManagerAccommodationsWithRoomIds(int contractManagerId)
                => await _dbContext.Accommodations
                    .Include(accommodation => accommodation.Rooms)
                    .Where(accommodation => accommodation.ContractManagerId == contractManagerId)
                    .OrderBy(accommodation => accommodation.Id)
                    .Skip(skip)
                    .Take(top)
                    .ToListAsync();
            
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
                    return Build(entry.Entity);
                });
        }


        public Task<Result<Models.Responses.Accommodation>> Update(int accommodationId, Models.Requests.Accommodation accommodation)
        {
            return ValidateAccommodation(accommodation)
                .Bind(() => _contractManagerContext.GetContractManager())
                .EnsureAccommodationBelongsToContractManager(_dbContext, accommodationId)
                .Map(Update)
                .Map(dbAccommodation => Build(dbAccommodation));


            async Task<Accommodation> Update(ContractManager contractManager)
            {
                var accommodationRecord = CreateAccommodation(contractManager.Id, accommodation);
                accommodationRecord.Id = accommodationId;
                var entry = _dbContext.Accommodations.Update(accommodationRecord);
                await _dbContext.SaveChangesAsync();
                
                _dbContext.DetachEntry(entry.Entity);

                return entry.Entity;
            }
        }


        public async Task<Result> Remove(int accommodationId)
        {
            //TODO Try to find out why MAP doesn't work here:
            /*
             Cannot access a disposed object. A common cause of this error is disposing a context that was resolved from dependency injection and then later trying to use the same context instance elsewhere in your application. 
             This may occur if you are calling Dispose() on the context, or wrapping the context in a using statement. 
             If you are using dependency injection, you should let the dependency injection container take care of disposing context instances.
             Object name: 'DirectContractsDbContext'.
             */
            return await _contractManagerContext.GetContractManager()
                .Tap(contractManager => RemoveAccommodationWithRooms(contractManager.Id));
                
            
            async Task RemoveAccommodationWithRooms(int contractManagerId)
            {
                var accommodation = await GetAccommodationWithRooms(contractManagerId, accommodationId).SingleOrDefaultAsync();

                if (accommodation == null)
                    return;
            
                if (accommodation.Rooms.Any())
                {
                    _dbContext.Rooms.RemoveRange(accommodation.Rooms);
                }

                _dbContext.Accommodations.Remove(accommodation);

                await _dbContext.SaveChangesAsync();
            }
        }


        public Task<Result<List<Models.Responses.Room>>> GetRooms(int accommodationId, int skip, int top) 
            => _contractManagerContext.GetContractManager()
            .Map(async contractManager =>
            {
                var rooms = await GetRooms(contractManager.Id, accommodationId)
                    .OrderBy(room => room.Id).Skip(skip).Take(top)
                    .ToListAsync();
                
                return rooms;
            })
            .Map(Build);


        public Task<Result<Models.Responses.Room>> GetRoom(int accommodationId, int roomId)
        {
            return  _contractManagerContext.GetContractManager()
                .Bind(contractManager => GetRoom(contractManager.Id))
                .Map(Build);


            async Task<Result<Room>> GetRoom(int contractManagerId)
            {
                var room = await _dbContext.GetAccommodations()
                    .Where(accommodation => accommodation.Id == accommodationId &&
                        accommodation.ContractManagerId == contractManagerId)
                    .Select(accommodation => accommodation.Rooms.SingleOrDefault(room => room.Id == roomId))
                    .SingleOrDefaultAsync();
                
                return room == null 
                    ? Result.Failure<Room>($"Failed to get the room with {nameof(roomId)} '{roomId}'") 
                    : Result.Success(room);
            }
        }
        

        public Task<Result<Models.Responses.Room>> UpdateRoom(int accommodationId, int roomId, Models.Requests.Room room)
        {
            return ValidateRoom()
                .Bind(() => _contractManagerContext.GetContractManager())
                .EnsureAccommodationBelongsToContractManager(_dbContext, accommodationId)
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
                    
                    return Build(dbRoom);
                }
        }
        

        public Task<Result<List<Models.Responses.Room>>> AddRooms(int accommodationId, List<Models.Requests.Room> rooms)
        {
            return ValidationHelper.Validate(rooms, new RoomValidator())
                .Bind(() => _contractManagerContext.GetContractManager())
                .EnsureAccommodationBelongsToContractManager(_dbContext, accommodationId)
                .Map(async contractManager =>
                {
                    var newRooms = CreateRooms(accommodationId, rooms); 
                    _dbContext.Rooms.AddRange(newRooms);
                    await _dbContext.SaveChangesAsync();
                    _dbContext.DetachEntries(newRooms);

                    return Build(newRooms);
                });
        }


        public async Task<Result> RemoveRooms(int accommodationId, List<int> roomIds)
        {
            return await _contractManagerContext.GetContractManager()
                .Map(contractManager => GetValidRooms(contractManager.Id))
                .Tap(Remove);

                
            async Task<List<Room>> GetValidRooms(int contractManagerId)
            {
                var rooms = await GetRooms(contractManagerId, accommodationId).ToListAsync();
                if (!rooms.Any())
                    return new List<Room>();

                var ids = rooms.Select(r => r.Id).Intersect(roomIds).ToList();

                return rooms.Where(room => ids.Contains(room.Id)).ToList();
            }


            async Task Remove(List<Room> rooms)
            {
                _dbContext.Rooms.RemoveRange(rooms);

                await _dbContext.SaveChangesAsync();
            }
        }


        private IQueryable<Room> GetRooms(int contractManagerId, int accommodationId)
            => _dbContext.Rooms
                .Join(_dbContext.Accommodations, room => room.AccommodationId, accommodation => accommodation.Id,
                    (room, accommodation) => new {room, accommodation})
                .Where(roomAndAccommodations =>
                    roomAndAccommodations.accommodation.ContractManagerId == contractManagerId &&
                    roomAndAccommodations.accommodation.Id == accommodationId)
                .Select(roomAndAccommodation => roomAndAccommodation.room);
       
        

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


        private Models.Responses.Accommodation Build(Accommodation accommodation)
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
                roomIds: accommodation.Rooms != null ? accommodation.Rooms.Select(room => room.Id).ToList() : new List<int>());
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
        

        private static Models.Responses.Room Build(Room room)
            => new Models.Responses.Room(room.Id, room.Name.GetValue<MultiLanguage<string>>(),
                room.Description.GetValue<MultiLanguage<string>>(), room.Amenities.GetValue<MultiLanguage<List<string>>>(),
                room.Pictures.GetValue<MultiLanguage<List<Picture>>>(), room.OccupancyConfigurations);
        

        private static List<Models.Responses.Room> Build(List<Room> rooms) 
            => rooms.Select(Build).ToList();


        private async Task<bool> DoesRoomBelongToAccommodation(int accommodationId, int roomId) 
            => await _dbContext.Rooms.Where(room => room.AccommodationId == accommodationId && room.Id == roomId).SingleOrDefaultAsync() != null;


        private IQueryable<Accommodation> GetAccommodationWithRooms(int contractManagerId, int accommodationId) 
            => _dbContext.Accommodations
            .Include(accommodation => accommodation.Rooms)
            .Where(accommodation => accommodation.ContractManagerId == contractManagerId &&
                accommodation.Id == accommodationId);


        private readonly IContractManagerContextService _contractManagerContext;
        private readonly GeometryFactory _geometryFactory;
        private readonly DirectContractsDbContext _dbContext;
    }
}