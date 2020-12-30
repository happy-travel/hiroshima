using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Geography;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.Data.Extensions;
using HappyTravel.Hiroshima.DirectManager.Infrastructure;
using HappyTravel.Hiroshima.DirectManager.RequestValidators;
using Microsoft.EntityFrameworkCore;
using Accommodation = HappyTravel.Hiroshima.Common.Models.Accommodations.Accommodation;
using NetTopologySuite.Geometries;
using Room = HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.Room;
using HappyTravel.Hiroshima.Common.Infrastructure;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public class AccommodationManagementService : IAccommodationManagementService
    {
        public AccommodationManagementService(IManagerContextService managerContextService, 
            IServiceSupplierContextService serviceSupplierContextService,
            IImageManagementService imageManagementService, 
            IAmenityService amenityService,
            DirectContractsDbContext dbContext, 
            GeometryFactory geometryFactory,
            IDateTimeProvider dateTimeProvider)
        {
            _managerContext = managerContextService;
            _serviceSupplierContext = serviceSupplierContextService;
            _imageManagementService = imageManagementService;
            _amenityService = amenityService;
            _geometryFactory = geometryFactory;
            _dbContext = dbContext;
            _dateTimeProvider = dateTimeProvider;
        }


        public Task<Result<List<Models.Responses.Accommodation>>> GetAccommodations(int contractId)
        {
            return _managerContext.GetServiceSupplier()
                .Check(serviceSupplier =>_serviceSupplierContext.EnsureContractBelongsToServiceSupplier(serviceSupplier, contractId))
                .Map(serviceSupplier => GetContractAccommodations())
                .Map(Build);


            Task<List<Accommodation>> GetContractAccommodations()
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
            return _managerContext.GetServiceSupplier()
                .Check(serviceSupplier => _serviceSupplierContext.EnsureAccommodationBelongsToServiceSupplier(serviceSupplier, accommodationId))
                .Bind(async serviceSupplier =>
                {
                    var accommodation = await GetAccommodationWithRooms(serviceSupplier.Id, accommodationId).SingleOrDefaultAsync();
                    if (accommodation == null)
                        return Result.Failure<Models.Responses.Accommodation>(
                            $"Failed to get an accommodation by {nameof(accommodationId)} '{accommodationId}'");
                    
                    return Build(accommodation);
                });
        }


        public Task<Result<List<Models.Responses.Accommodation>>> Get(int skip, int top)
        {
            return _managerContext.GetServiceSupplier()
                .Map(serviceSupplier => GetAccommodationsWithRooms(serviceSupplier.Id))
                .Map(accommodations =>
                    accommodations.Select(Build).ToList()
                );


            async Task<List<Accommodation>> GetAccommodationsWithRooms(int serviceSupplierId)
                => await _dbContext.Accommodations
                    .Include(accommodation => accommodation.Rooms)
                    .Where(accommodation => accommodation.ServiceSupplierId == serviceSupplierId)
                    .OrderBy(accommodation => accommodation.Id)
                    .Skip(skip)
                    .Take(top)
                    .ToListAsync();
        }


        public async Task<Result<Accommodation>> GetInternal(int accommodationId)
        {
            var accommodation = await _dbContext.Accommodations
                .Include(a => a.Location)
                .ThenInclude(location => location.Country)
                .Include(a => a.Rooms)
                .SingleOrDefaultAsync(a => a.Id == accommodationId);

            return accommodation != null
                ? Result.Success(accommodation)
                : Result.Failure<Accommodation>("Failed to retrieve the accommodation");
        }


        public Task<Result<Models.Responses.Accommodation>> Add(Models.Requests.Accommodation accommodationRequest)
        {
            return ValidationHelper.Validate(accommodationRequest, new AccommodationValidator(_dateTimeProvider))
                .Bind(() => _managerContext.GetServiceSupplier())
                .Map(serviceSupplier => CreateAccommodation(serviceSupplier.Id, accommodationRequest))
                .Map(NormalizeAccommodationAmenities)
                .Map(AddAccommodation)
                .Tap(AddAccommodationAmenitiesToStoreIfNeeded)
                .Map(Build);


            async Task<Accommodation> AddAccommodation(Accommodation accommodation)
            {
                accommodation.Created = _dateTimeProvider.UtcNow();

                var entry = _dbContext.Accommodations.Add(accommodation);
                await _dbContext.SaveChangesAsync();
                entry.State = EntityState.Detached;

                return entry.Entity;
            }
        }


        public Task<Result<Models.Responses.Accommodation>> Update(int accommodationId, Models.Requests.Accommodation accommodationRequest)
        {
            return ValidationHelper.Validate(accommodationRequest, new AccommodationValidator(_dateTimeProvider))
                .Bind(() => _managerContext.GetServiceSupplier())
                .Check(serviceSupplier => _serviceSupplierContext.EnsureAccommodationBelongsToServiceSupplier(serviceSupplier, accommodationId))
                .Map(serviceSupplier => CreateAccommodation(serviceSupplier.Id, accommodationRequest))
                .Map(NormalizeAccommodationAmenities)
                .Map(UpdateAccommodation)
                .Tap(AddAccommodationAmenitiesToStoreIfNeeded)
                .Map(Build);


            async Task<Accommodation> UpdateAccommodation(Accommodation accommodation)
            {
                accommodation.Id = accommodationId;

                var entry = _dbContext.Accommodations.Update(accommodation);
                await _dbContext.SaveChangesAsync();

                _dbContext.DetachEntry(entry.Entity);

                return entry.Entity;
            }
        }


        public async Task<Result> Remove(int accommodationId)
        {
            //TODO Try to find out why MAP doesn't work here:
            //https://happytravel.atlassian.net/browse/HIR-74
            return await _managerContext.GetServiceSupplier()
                .Check(serviceSupplier => _serviceSupplierContext.EnsureAccommodationBelongsToServiceSupplier(serviceSupplier, accommodationId))
                .Check(serviceSupplier => RemoveAccommodationImages(serviceSupplier.Id))
                .Tap(serviceSupplier => RemoveAccommodationWithRooms(serviceSupplier.Id));


            async Task<Result> RemoveAccommodationImages(int serviceSupplierId)
            {
                return await _imageManagementService.RemoveAll(serviceSupplierId, accommodationId);
            }


            async Task RemoveAccommodationWithRooms(int serviceSupplierId)
            {
                var accommodation = await GetAccommodationWithRooms(serviceSupplierId, accommodationId).SingleOrDefaultAsync();

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
            => _managerContext.GetServiceSupplier()
                .Check(serviceSupplier => _serviceSupplierContext.EnsureAccommodationBelongsToServiceSupplier(serviceSupplier, accommodationId))
                .Map(async serviceSupplier =>
                {
                    var rooms = await GetRooms(serviceSupplier.Id, accommodationId)
                        .OrderBy(room => room.Id).Skip(skip).Take(top)
                        .ToListAsync();

                    return rooms;
                })
                .Map(Build);


        public Task<Result<Models.Responses.Room>> GetRoom(int accommodationId, int roomId)
        {
            return _managerContext.GetServiceSupplier()
                .Check(serviceSupplier => _serviceSupplierContext.EnsureAccommodationBelongsToServiceSupplier(serviceSupplier, accommodationId))
                .Bind(serviceSupplier => GetRoom(serviceSupplier.Id))
                .Map(Build);


            async Task<Result<Room>> GetRoom(int serviceSupplierId)
            {
                var room = await _dbContext.GetAccommodations()
                    .Where(accommodation => accommodation.Id == accommodationId &&
                        accommodation.ServiceSupplierId == serviceSupplierId)
                    .Select(accommodation => accommodation.Rooms.SingleOrDefault(r => r.Id == roomId))
                    .SingleOrDefaultAsync();

                return room == null
                    ? Result.Failure<Room>($"Failed to get the room with {nameof(roomId)} '{roomId}'")
                    : Result.Success(room);
            }
        }


        public Task<Result<Models.Responses.Room>> UpdateRoom(int accommodationId, int roomId, Models.Requests.Room roomRequest)
        {
            return ValidateRoom()
                .Bind(() => _managerContext.GetServiceSupplier())
                .Check(serviceSupplier => _serviceSupplierContext.EnsureAccommodationBelongsToServiceSupplier(serviceSupplier, accommodationId))
                .Ensure(serviceSupplier => DoesRoomBelongToAccommodation(accommodationId, roomId), $"The room with {nameof(roomId)} '{roomId}' doesn't belong to the accommodation with {nameof(accommodationId)} '{accommodationId}'")
                .Map(serviceSupplier => CreateRoom(accommodationId, roomRequest))
                .Map(NormalizeRoomAmenities)
                .Map(UpdateRoom)
                .Tap(AddRoomAmenitiesToStoreIfNeeded)
                .Map(Build);


            Result ValidateRoom()
            => GenericValidator<Models.Requests.Room>.Validate(configureAction => configureAction.RuleFor(exp => exp)
                .SetValidator(new RoomValidator()), roomRequest);


            async Task<Room> UpdateRoom(Room room)
            {
                room.Id = roomId;

                _dbContext.Rooms.Update(room);
                await _dbContext.SaveChangesAsync();

                return room;
            }
        }


        public Task<Result<List<Models.Responses.Room>>> AddRooms(int accommodationId, List<Models.Requests.Room> roomsRequest)
        {
            return ValidationHelper.Validate(roomsRequest, new RoomValidator())
                .Bind(() => _managerContext.GetServiceSupplier())
                .Check(serviceSupplier => _serviceSupplierContext.EnsureAccommodationBelongsToServiceSupplier(serviceSupplier, accommodationId))
                .Map(serviceSupplier => CreateRooms(accommodationId, roomsRequest))
                .Map(NormalizeRoomsAmenities)
                .Map(AddRooms)
                .Tap(AddRoomsAmenitiesToStoreIfNeeded)
                .Map(Build);

                
            async Task<List<Room>> AddRooms(List<Room> rooms)
            {
                var utcNow = _dateTimeProvider.UtcNow();
                rooms.ForEach(room => room.Created = utcNow);

                _dbContext.Rooms.AddRange(rooms);
                await _dbContext.SaveChangesAsync();
                _dbContext.DetachEntries(rooms);

                return rooms;
            }
        }


        public async Task<Result> RemoveRooms(int accommodationId, List<int> roomIds)
        {
            return await _managerContext.GetServiceSupplier()
                .Check(serviceSupplier => _serviceSupplierContext.EnsureAccommodationBelongsToServiceSupplier(serviceSupplier, accommodationId))
                .Map(serviceSupplier => GetValidRooms(serviceSupplier.Id))
                .Tap(Remove);


            async Task<List<Room>> GetValidRooms(int serviceSupplierId)
            {
                var rooms = await GetRooms(serviceSupplierId, accommodationId).ToListAsync();
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


        private IQueryable<Room> GetRooms(int serviceSupplierId, int accommodationId)
            => _dbContext.Rooms
                .Join(_dbContext.Accommodations, room => room.AccommodationId, accommodation => accommodation.Id,
                    (room, accommodation) => new { room, accommodation })
                .Where(roomAndAccommodations =>
                    roomAndAccommodations.accommodation.ServiceSupplierId == serviceSupplierId &&
                    roomAndAccommodations.accommodation.Id == accommodationId)
                .Select(roomAndAccommodation => roomAndAccommodation.room);


        private Accommodation CreateAccommodation(int serviceSupplierId, Models.Requests.Accommodation accommodation)
        {
            return new Accommodation
            {
                Name = accommodation.Name,
                Address = accommodation.Address,
                Coordinates = _geometryFactory.CreatePoint(new Coordinate(accommodation.Coordinates.Longitude, accommodation.Coordinates.Latitude)),
                AccommodationAmenities = accommodation.Amenities,
                TextualDescription = accommodation.Description,
                LeisureAndSports = accommodation.LeisureAndSports,
                Rating = accommodation.Rating,
                ServiceSupplierId = serviceSupplierId,
                ContactInfo = new ContactInfo
                {
                    Emails = accommodation.ContactInfo.Emails,
                    Phones = accommodation.ContactInfo.Phones,
                    Websites = accommodation.ContactInfo.Websites
                },
                AdditionalInfo = accommodation.AdditionalInfo,
                OccupancyDefinition = accommodation.OccupancyDefinition,
                PropertyType = accommodation.Type,
                CheckInTime = accommodation.CheckInTime,
                CheckOutTime = accommodation.CheckOutTime,
                LocationId = accommodation.LocationId,
                RateOptions = accommodation.RateOptions,
                Modified = _dateTimeProvider.UtcNow(),
                Status = accommodation.Status,
                Floors = accommodation.Floors,
                BuildYear = accommodation.BuildYear,
                PostalCode = accommodation.PostalCode
            };
        }


        private Accommodation NormalizeAccommodationAmenities(Accommodation accommodation)
        {
            accommodation.AccommodationAmenities = _amenityService.Normalize(accommodation.AccommodationAmenities);

            return accommodation;
        }


        private async Task<Accommodation> AddAccommodationAmenitiesToStoreIfNeeded(Accommodation accommodation)
        {
            await _amenityService.Update(accommodation.AccommodationAmenities);
            return accommodation;
        }


        private Models.Responses.Accommodation Build(Accommodation accommodation)
        {
            return new Models.Responses.Accommodation(
                accommodation.Id,
                accommodation.Name,
                accommodation.Address,
                accommodation.TextualDescription,
                 new GeoPoint(accommodation.Coordinates),
                accommodation.Rating,
                accommodation.CheckInTime,
                accommodation.CheckOutTime,
                new Models.Responses.ContactInfo(accommodation.ContactInfo.Emails, accommodation.ContactInfo.Phones, accommodation.ContactInfo.Websites), 
                accommodation.PropertyType,
                accommodation.AccommodationAmenities,
                accommodation.AdditionalInfo,
                accommodation.OccupancyDefinition,
                accommodation.LocationId,
                accommodation.LeisureAndSports,
                accommodation.Status,
                accommodation.RateOptions,
                accommodation.Floors,
                accommodation.BuildYear,
                accommodation.PostalCode,
                accommodation.Rooms != null
                    ? accommodation.Rooms.Select(Build).ToList()
                    : new List<Models.Responses.Room>());
        }


        private List<Models.Responses.Accommodation> Build(List<Accommodation> accommodations) =>
            accommodations.Select(Build).ToList();


        private List<Room> CreateRooms(int accommodationId, List<Models.Requests.Room> rooms)
            => rooms.Select(room => CreateRoom(accommodationId, room))
            .ToList();


        private Room CreateRoom(int accommodationId, Models.Requests.Room room) => new Room
        {
            AccommodationId = accommodationId,
            Name = room.Name,
            Description = room.Description,
            Amenities = room.Amenities,
            Modified = _dateTimeProvider.UtcNow(),
            OccupancyConfigurations = room.OccupancyConfigurations
        };


        private List<Room> NormalizeRoomsAmenities(List<Room> rooms) =>
            rooms.Select(NormalizeRoomAmenities).ToList();
        
        
        private Room NormalizeRoomAmenities(Room room)
        {
            room.Amenities = _amenityService.Normalize(room.Amenities);

            return room;
        }


        private async Task<List<Room>> AddRoomsAmenitiesToStoreIfNeeded(List<Room> rooms)
        {
            foreach (var room in rooms)
                await AddRoomAmenitiesToStoreIfNeeded(room);

            return rooms;
        }

        private async Task<Room> AddRoomAmenitiesToStoreIfNeeded(Room room)
        {
            await _amenityService.Update(room.Amenities);
            
            return room;
        }


        private static Models.Responses.Room Build(Room room)
            => new Models.Responses.Room(
                room.Id, 
                room.Name,
                room.Description, 
                room.Amenities,
                room.OccupancyConfigurations);


        private static List<Models.Responses.Room> Build(List<Room> rooms)
            => rooms.Select(Build).ToList();


        private async Task<bool> DoesRoomBelongToAccommodation(int accommodationId, int roomId)
            => await _dbContext.Rooms.Where(room => room.AccommodationId == accommodationId && room.Id == roomId).SingleOrDefaultAsync() != null;


        private IQueryable<Accommodation> GetAccommodationWithRooms(int serviceSupplierId, int accommodationId)
            => _dbContext.Accommodations
            .Include(accommodation => accommodation.Rooms)
            .Where(accommodation => accommodation.ServiceSupplierId == serviceSupplierId &&
                accommodation.Id == accommodationId);


        private readonly IManagerContextService _managerContext;
        private readonly IServiceSupplierContextService _serviceSupplierContext;
        private readonly IImageManagementService _imageManagementService;
        private readonly IAmenityService _amenityService;
        private readonly GeometryFactory _geometryFactory;
        private readonly DirectContractsDbContext _dbContext;
        private readonly IDateTimeProvider _dateTimeProvider;
    }
}