using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HappyTravel.EdoContracts.Accommodations;
using HappyTravel.EdoContracts.Accommodations.Internals;
using HappyTravel.EdoContracts.GeoData.Enums;
using HappyTravel.Hiroshima.Common.Constants;
using HappyTravel.Hiroshima.Common.Infrastructure.Utilities;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms;
using HappyTravel.Hiroshima.Common.Models.Availabilities;
using HappyTravel.Hiroshima.Data;
using Microsoft.EntityFrameworkCore;
using Accommodation = HappyTravel.Hiroshima.Common.Models.Accommodations.Accommodation;

namespace HappyTravel.Hiroshima.DirectContracts.Services.Availability
{
    public class AvailabilityService : IAvailabilityService
    {
        public AvailabilityService(IRoomAvailabilityService roomAvailabilityService, IRateAvailabilityService rateAvailabilityService, IAvailabilityDataStorage availabilityDataStorage, IAvailabilityIdGenerator availabilityIdGenerator, IRateDetailsSetsGenerator rateDetailsSetsGenerator, DirectContractsDbContext dbContext)
        {
            _roomAvailabilityService = roomAvailabilityService;
            _rateAvailabilityService = rateAvailabilityService;
            _availabilityDataStorage = availabilityDataStorage;
            _availabilityIdGenerator = availabilityIdGenerator;
            _rateDetailsSetsGenerator = rateDetailsSetsGenerator;
            _dbContext = dbContext;
        }


        public async Task<Common.Models.Availabilities.Availability> Get(AvailabilityRequest availabilityRequest, string languageCode)
        {
            var accommodations = await ExtractAvailabilityData();
            var groupedAvailableRooms = _roomAvailabilityService.GetGroupedAvailableRooms(availabilityRequest, accommodations, availabilityRequest.Rooms);

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
                            => accommodation.Location.Country.Name.RootElement.GetProperty(Languages.GetLanguageCode(Languages.DefaultLanguage)).GetString() ==
                            location.Country &&
                            accommodation.Location.Locality.RootElement.GetProperty(Languages.GetLanguageCode(Languages.DefaultLanguage)).GetString() ==
                            location.Locality;
                        break;
                    }
                    case LocationTypes.Accommodation:
                    {
                        locationExpression = accommodation
                            => accommodation.Name.RootElement.GetProperty(Languages.GetLanguageCode(Languages.DefaultLanguage)).GetString() == location.Name;
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
            var groupedAvailableRooms = _roomAvailabilityService.GetGroupedAvailableRooms(availabilityRequest, accommodations, availabilityRequest.Rooms);

            var availability = CreateAvailability(availabilityRequest, groupedAvailableRooms, languageCode);

            await _availabilityDataStorage.Add(availability);

            return availability;
        }
        
        
        private IQueryable<Accommodation> GetAvailableAccommodations(AvailabilityRequest availabilityRequest)
        {
            var roomTypes = availabilityRequest.Rooms.Select(room => room.Type).ToList();
            var ratings = AccommodationRatingConverter.Convert(availabilityRequest.Ratings);
            var checkInDate = availabilityRequest.CheckInDate.Date;
            var checkOutDate = availabilityRequest.CheckOutDate.Date;

            return _dbContext.Accommodations
                .Include(accommodation => accommodation.Rooms.Where(room => !room.AvailabilityRestrictions.Any(availabilityRestrictions
                    => checkInDate <= availabilityRestrictions.ToDate && availabilityRestrictions.FromDate <= checkOutDate &&
                    availabilityRestrictions.Restriction == AvailabilityRestrictions.StopSale)))
                .ThenInclude(room => room.AllocationRequirements.Where(allocationRequirements
                    => checkInDate <= allocationRequirements.SeasonRange.EndDate && allocationRequirements.SeasonRange.StartDate <= checkOutDate))
                .ThenInclude(allocationRequirements => allocationRequirements.SeasonRange)
                .Include(accommodation => accommodation.Rooms)
                .ThenInclude(room => room.AvailabilityRestrictions.Where(availabilityRestrictions
                    => checkInDate <= availabilityRestrictions.ToDate && availabilityRestrictions.FromDate <= checkOutDate))
                .Include(accommodation => accommodation.Rooms)
                .ThenInclude(room => room.CancellationPolicies.Where(cancellationPolicy
                    => cancellationPolicy.Season.SeasonRanges.Any(seasonRange => checkInDate <= seasonRange.EndDate && seasonRange.StartDate <= checkOutDate)))
                .ThenInclude(cancellationPolicy => cancellationPolicy.Season)
                .ThenInclude(season => season.SeasonRanges.Where(seasonRange => checkInDate <= seasonRange.EndDate && seasonRange.StartDate <= checkOutDate))
                .Include(accommodation => accommodation.Rooms)
                .ThenInclude(room => room.RoomRates.Where(rate
                    => roomTypes.Contains(rate.RoomType) &&
                    rate.Season.SeasonRanges.Any(seasonRange => checkInDate <= seasonRange.EndDate && seasonRange.StartDate <= checkOutDate)))
                .ThenInclude(rate => rate.Season)
                .ThenInclude(season => season.SeasonRanges.Where(seasonRange => checkInDate <= seasonRange.EndDate && seasonRange.StartDate <= checkOutDate))
                .Include(accommodation => accommodation.Rooms)
                .ThenInclude(room => room.RoomPromotionalOffers.Where(promotionalOffer
                    => checkInDate <= promotionalOffer.ValidToDate && promotionalOffer.ValidFromDate <= checkOutDate))
                .Include(accommodation => accommodation.Rooms)
                .ThenInclude(room => room.RoomOccupations.Where(roomOccupation => checkInDate <= roomOccupation.FromDate && roomOccupation.ToDate <= checkOutDate))
                .Include(accommodation => accommodation.Location)
                .ThenInclude(location => location.Country)
                .Where(accommodation => ratings.Contains(accommodation.Rating));
        }

        
        private Common.Models.Availabilities.Availability CreateAvailability(AvailabilityRequest availabilityRequest, List<Dictionary<RoomOccupationRequest, List<Room>>> groupedAvailableRooms, string languageCode)
        {
            var accommodationAvailableRates = new Dictionary<Accommodation, List<AvailableRates>>();
            foreach (var accommodationGroupedRooms in groupedAvailableRooms)
            {
                if (accommodationGroupedRooms.Count != availabilityRequest.Rooms.Count)
                    continue;
                    
                var availableRateDetails = GetAvailableRateDetails(accommodationGroupedRooms);

                var rateDetailsSets = _rateDetailsSetsGenerator.GenerateSets(availabilityRequest, availableRateDetails);
                if (!rateDetailsSets.Any())
                    continue;

                var accommodation = accommodationGroupedRooms.First().Value.First().Accommodation;
                accommodationAvailableRates.Add(accommodation, rateDetailsSets.Select(rateDetails => new AvailableRates
                {
                    Id = GenerateRateDetailsId(),
                    AccommodationId = accommodation.Id,
                    Hash = _availabilityIdGenerator.Generate(rateDetails),
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
        private readonly IAvailabilityIdGenerator _availabilityIdGenerator;
        private readonly IRateDetailsSetsGenerator _rateDetailsSetsGenerator;
    }
}