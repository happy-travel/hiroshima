using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HappyTravel.EdoContracts.Accommodations;
using HappyTravel.EdoContracts.Accommodations.Enums;
using HappyTravel.EdoContracts.Accommodations.Internals;
using HappyTravel.EdoContracts.GeoData.Enums;
using HappyTravel.Hiroshima.Common.Infrastructure.Utilities;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms;
using HappyTravel.Hiroshima.Common.Models.Availabilities;
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.Data.Models;
using HappyTravel.Hiroshima.DirectContracts.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Accommodation = HappyTravel.Hiroshima.Common.Models.Accommodations.Accommodation;

namespace HappyTravel.Hiroshima.DirectContracts.Services.Availability
{
    public class AvailabilityService : IAvailabilityService
    {
        public AvailabilityService(IRoomAvailabilityService roomAvailabilityService, IRateAvailabilityService rateAvailabilityService, IAvailabilityDataStorage availabilityDataStorage, IAvailabilityHashGenerator availabilityHashGenerator, IRateDetailsSetGenerator rateDetailsSetGenerator, DirectContractsDbContext dbContext)
        {
            _roomAvailabilityService = roomAvailabilityService;
            _rateAvailabilityService = rateAvailabilityService;
            _availabilityDataStorage = availabilityDataStorage;
            _availabilityHashGenerator = availabilityHashGenerator;
            _rateDetailsSetGenerator = rateDetailsSetGenerator;
            _dbContext = dbContext;
        }


        public async Task<Common.Models.Availabilities.Availability> Get(AvailabilityRequest availabilityRequest, string languageCode)
        {
            var accommodations = await ExtractAvailabilityData();
            var groupedAvailableRooms = _roomAvailabilityService.GetGroupedAvailableRooms(availabilityRequest, accommodations);
            
            return CreateAvailability(availabilityRequest, groupedAvailableRooms, languageCode);

            
            async Task<List<Accommodation>> ExtractAvailabilityData()
            {
                var location = availabilityRequest.Location;
                
                Expression<Func<Accommodation, bool>> locationExpression;
                
                switch (availabilityRequest.Location.Type)
                {
                    case LocationTypes.Location:
                    {
                        locationExpression = accommodation
                            => accommodation.Location.Country.Name.En == location.Country &&
                            accommodation.Location.Locality.En == location.Locality;
                        break;
                    }
                    case LocationTypes.Accommodation:
                    {
                        locationExpression = accommodation
                            => accommodation.Name.En == location.Name;
                        break;
                    }
                    case LocationTypes.Unknown:
                    case LocationTypes.Destination:
                    case LocationTypes.Landmark:
                        throw new NotImplementedException("Not implemented yet");
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                return await GetAvailableAccommodations(availabilityRequest)
                    .Where(locationExpression)
                    .ToListAsync();
            }
        }


        public async Task<Common.Models.Availabilities.Availability> Get(AvailabilityRequest availabilityRequest, int accommodationId,
            string languageCode)
        {
            var accommodations = await GetAvailableAccommodations(availabilityRequest)
                .Where(accommodation => accommodation.Id.Equals(accommodationId))
                .ToListAsync();
            var groupedAvailableRooms = _roomAvailabilityService.GetGroupedAvailableRooms(availabilityRequest, accommodations);

            var availability = CreateAvailability(availabilityRequest, groupedAvailableRooms, languageCode);

            await _availabilityDataStorage.Add(availability);

            return availability;
        }
        
        
        private IQueryable<Accommodation> GetAvailableAccommodations(AvailabilityRequest availabilityRequest)
        {
            var roomTypes = GetRoomTypes(availabilityRequest);
            var ratings = AccommodationRatingConverter.Convert(availabilityRequest.Ratings);

            return _dbContext.ContractAccommodationRelations
                .IncludeAllocationRequirements(availabilityRequest)
                .IncludeAvailabilityRestrictions(availabilityRequest)
                .IncludeCancellationPolicies(availabilityRequest)
                .IncludeRates(availabilityRequest, roomTypes)
                .IncludePromotionalOffers(availabilityRequest)
                .IncludeRoomOccupations(availabilityRequest)
                .IncludeLocation()
                .Select(relation => relation.Accommodation)
                .Where(accommodation => ratings.Contains(accommodation.Rating));
        }
            
        
        private List<RoomTypes> GetRoomTypes(AvailabilityRequest availabilityRequest)
        {
            var roomTypes = availabilityRequest.Rooms.Select(room => room.Type).Distinct().ToList();
            if (roomTypes.All(rt => rt == RoomTypes.NotSpecified))
                roomTypes = Enum.GetValues(typeof(RoomTypes)).Cast<RoomTypes>().ToList();
            
            return roomTypes;
        }
        
        
        private Common.Models.Availabilities.Availability CreateAvailability(AvailabilityRequest availabilityRequest, List<Dictionary<RoomOccupationRequest, List<Room>>> groupedAvailableRooms, string languageCode)
        {
            var accommodationAvailableRates = new Dictionary<Accommodation, List<AvailableRates>>();
            foreach (var accommodationGroupedRooms in groupedAvailableRooms)
            {
                if (accommodationGroupedRooms.Count != availabilityRequest.Rooms.Count)
                    continue;
                    
                var availableRateDetails = GetAvailableRateDetails(accommodationGroupedRooms);

                var rateDetailsSets = _rateDetailsSetGenerator.GenerateSets(availabilityRequest, availableRateDetails);
                if (!rateDetailsSets.Any())
                    continue;

                var accommodation = accommodationGroupedRooms.First().Value.First().Accommodation;
                accommodationAvailableRates.Add(accommodation, rateDetailsSets.Select(rateDetails => new AvailableRates
                {
                    Id = GenerateRateDetailsId(),
                    AccommodationId = accommodation.Id,
                    Hash = _availabilityHashGenerator.Generate(rateDetails),
                    Rates = rateDetails
                }).ToList());
            }
            
            return new Common.Models.Availabilities.Availability
            {
                Id = GenerateAvailability(),
                AvailableRates = accommodationAvailableRates
            };

            
            Dictionary<RoomOccupationRequest, List<RateDetails>> GetAvailableRateDetails(Dictionary<RoomOccupationRequest, List<Room>> occupationRequestRoomsStore)
                => occupationRequestRoomsStore
                    .ToDictionary(occupationRequestRooms => occupationRequestRooms.Key, occupationRequestRooms => _rateAvailabilityService.GetAvailableRates(occupationRequestRooms.Key, occupationRequestRooms.Value.ToList(), availabilityRequest.CheckInDate, availabilityRequest.CheckOutDate, languageCode));
            
            
            string GenerateAvailability() => Guid.NewGuid().ToString("N");
            
            
            Guid GenerateRateDetailsId() => Guid.NewGuid();
        }


        private readonly IRateAvailabilityService _rateAvailabilityService;
        private readonly IRoomAvailabilityService _roomAvailabilityService;
        private readonly DirectContractsDbContext _dbContext;
        private readonly IAvailabilityDataStorage _availabilityDataStorage;
        private readonly IAvailabilityHashGenerator _availabilityHashGenerator;
        private readonly IRateDetailsSetGenerator _rateDetailsSetGenerator;
    }
}