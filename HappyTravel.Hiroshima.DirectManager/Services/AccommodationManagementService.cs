using System;
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
using HappyTravel.Hiroshima.DirectManager.Infrastructure;
using HappyTravel.Hiroshima.DirectManager.Infrastructure.Extensions;
using HappyTravel.Hiroshima.DirectManager.RequestValidators;
using Microsoft.EntityFrameworkCore;
using Accommodation = HappyTravel.Hiroshima.Common.Models.Accommodations.Accommodation;
using NetTopologySuite.Geometries;
using Room = HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.Room;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public class AccommodationManagementService : IAccommodationManagementService
    {
        public AccommodationManagementService(
            IContractManagerContextService contractManagerContext, IImageManagementService imageManagementService, DirectContractsDbContext dbContext, GeometryFactory geometryFactory)
        {
            _contractManagerContext = contractManagerContext;
            _imageManagementService = imageManagementService;
            _geometryFactory = geometryFactory;
            _dbContext = dbContext;
        }


        public Task<Result<List<Models.Responses.Accommodation>>> GetAccommodations(int contractId)
        {
            return _contractManagerContext.GetContractManager()
                .EnsureContractBelongsToContractManager(_dbContext, contractId)
                .Map(contractManager => GetContractAccommodations(contractManager.Id))
                .Map(Build);


            Task<List<Accommodation>> GetContractAccommodations(int contractManagerId)
            {
                return _dbContext.Accommodations.Include(accommodation => accommodation.Rooms)
                    .Join(_dbContext.ContractAccommodationRelations, accommodation => accommodation.Id, relation => relation.AccommodationId,
                        (accommodation, relation) => new { accommodation, relation })
                    .Where(accommodationAndRelation => accommodationAndRelation.relation.ContractId == contractId)
                    .Select(accommodationAndRelation => accommodationAndRelation.accommodation)
                    .ToListAsync();
            }
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
            return ValidationHelper.Validate(accommodation, new AccommodationValidator())
                .Bind(() => _contractManagerContext.GetContractManager())
                .Map(contractManager => AddAccommodation(contractManager.Id))
                .Map(Build);


            async Task<Accommodation> AddAccommodation(int contractManagerId)
            {
                var newAccommodation = CreateAccommodation(contractManagerId, accommodation);
                newAccommodation.Created = DateTime.UtcNow;
                var entry = _dbContext.Accommodations.Add(newAccommodation);
                await _dbContext.SaveChangesAsync();
                entry.State = EntityState.Detached;

                return entry.Entity;
            }
        }


        public Task<Result<Models.Responses.Accommodation>> Update(int accommodationId, Models.Requests.Accommodation accommodation)
        {
            return ValidationHelper.Validate(accommodation, new AccommodationValidator())
                .Bind(() => _contractManagerContext.GetContractManager())
                .EnsureAccommodationBelongsToContractManager(_dbContext, accommodationId)
                .Map(Update)
                .Map(Build);


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
            //https://happytravel.atlassian.net/browse/HIR-74
            return await _contractManagerContext.GetContractManager()
                .Tap(contractManager => RemoveAccommodationImages(contractManager.Id))
                .Tap(contractManager => RemoveAccommodationWithRooms(contractManager.Id));

            async Task<Result> RemoveAccommodationImages(int contractManagerId)
            {
                return await _imageManagementService.RemoveAll(contractManagerId, accommodationId);
            }

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
            return _contractManagerContext.GetContractManager()
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
                    var utcNow = DateTime.UtcNow;
                    newRooms.ForEach(room => room.Created = utcNow);
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
                    (room, accommodation) => new { room, accommodation })
                .Where(roomAndAccommodations =>
                    roomAndAccommodations.accommodation.ContractManagerId == contractManagerId &&
                    roomAndAccommodations.accommodation.Id == accommodationId)
                .Select(roomAndAccommodation => roomAndAccommodation.room);


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
                LeisureAndSports = JsonDocumentUtilities.CreateJDocument(accommodation.LeisureAndSports),
                Rating = accommodation.Rating,
                ContractManagerId = contractManagerId,
                ContactInfo = new ContactInfo
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
                RateOptions = accommodation.RateOptions,
                Modified = DateTime.UtcNow,
                Status = accommodation.Status,
                Floors = accommodation.Floors,
                BuildYear = accommodation.BuildYear
            };
        }


        private Models.Responses.Accommodation Build(Accommodation accommodation)
        {
            return new Models.Responses.Accommodation(
                accommodation.Id,
                accommodation.Name.GetValue<MultiLanguage<string>>(),
                accommodation.Address.GetValue<MultiLanguage<string>>(),
                accommodation.TextualDescription.GetValue<MultiLanguage<TextualDescription>>(),
                 new GeoPoint(accommodation.Coordinates),
                accommodation.Rating,
                accommodation.CheckInTime,
                accommodation.CheckOutTime,
                accommodation.Pictures.GetValue<MultiLanguage<List<Picture>>>(),
                accommodation.ContactInfo,
                accommodation.PropertyType,
                accommodation.AccommodationAmenities.GetValue<MultiLanguage<List<string>>>(),
                accommodation.AdditionalInfo.GetValue<MultiLanguage<string>>(),
                accommodation.OccupancyDefinition,
                accommodation.LocationId,
                accommodation.LeisureAndSports.GetValue<MultiLanguage<List<string>>>(),
                accommodation.Status,
                accommodation.RateOptions,
                accommodation.Floors,
                accommodation.BuildYear,
                accommodation.Rooms != null
                    ? accommodation.Rooms.Select(room => room.Id).ToList()
                    : new List<int>());
        }


        private List<Models.Responses.Accommodation> Build(List<Accommodation> accommodations) =>
            accommodations.Select(Build).ToList();


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
            Modified = DateTime.UtcNow,
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
        private readonly IImageManagementService _imageManagementService;
        private readonly GeometryFactory _geometryFactory;
        private readonly DirectContractsDbContext _dbContext;
    }
}