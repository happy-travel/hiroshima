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
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.DirectContracts.Models;
using Microsoft.EntityFrameworkCore;
using Accommodation = HappyTravel.Hiroshima.Common.Models.Accommodations.Accommodation;


namespace HappyTravel.Hiroshima.DirectContracts.Services.Availability
{
    public class AvailabilityService : IAvailabilityService
    {
        public AvailabilityService(IRoomAvailabilityService roomAvailabilityService, IRateAvailabilityService rateAvailabilityService, DirectContractsDbContext dbContext)
        {
            _roomAvailabilityService = roomAvailabilityService;
            _rateAvailabilityService = rateAvailabilityService;
            _dbContext = dbContext;
        }


        public async Task<Dictionary<Accommodation, List<AvailableRates>>> Get(AvailabilityRequest availabilityRequest, string languageCode)
        {
            var accommodations = await ExtractAvailabilityData();
            var groupedAvailableRooms = _roomAvailabilityService.GetGroupedAvailableRooms(accommodations, availabilityRequest.Rooms);

            return GetAvailableRates(availabilityRequest, groupedAvailableRooms, languageCode);
            
            
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


        public async Task<Dictionary<Accommodation, List<AvailableRates>>> Get(AvailabilityRequest availabilityRequest, int accommodationId, string languageCode)
        {
            var accommodations = await GetAvailableAccommodations(availabilityRequest)
                .Where(accommodation => accommodation.Id.Equals(accommodationId)).ToListAsync();
            var groupedAvailableRooms = _roomAvailabilityService.GetGroupedAvailableRooms(accommodations, availabilityRequest.Rooms);
            
            return GetAvailableRates(availabilityRequest, groupedAvailableRooms, languageCode);
        }
        
        
        private IQueryable<Accommodation> GetAvailableAccommodations(AvailabilityRequest availabilityRequest)
        {
            var roomTypes = availabilityRequest.Rooms.Select(room => room.Type).ToList();
            var ratings = AccommodationRatingMapper.GetStars(availabilityRequest.Ratings);
            var checkInDate = availabilityRequest.CheckInDate.Date;
            var checkOutDate = availabilityRequest.CheckOutDate.Date;

            return _dbContext.Accommodations
                .Include(accommodation => accommodation.Rooms.Where(room => !room.RoomAvailabilityRestrictions.Any(availabilityRestrictions
                    => checkInDate <= availabilityRestrictions.ToDate && availabilityRestrictions.FromDate <= checkOutDate &&
                    availabilityRestrictions.Restriction == AvailabilityRestrictions.StopSale)))
                .ThenInclude(room => room.RoomAllocationRequirements.Where(allocationRequirements
                    => checkInDate <= allocationRequirements.SeasonRange.EndDate && allocationRequirements.SeasonRange.StartDate <= checkOutDate))
                .ThenInclude(allocationRequirements => allocationRequirements.SeasonRange)
                .Include(accommodation => accommodation.Rooms)
                .ThenInclude(room => room.RoomAvailabilityRestrictions.Where(availabilityRestrictions
                    => checkInDate <= availabilityRestrictions.ToDate && availabilityRestrictions.FromDate <= checkOutDate))
                .Include(accommodation => accommodation.Rooms)
                .ThenInclude(room => room.RoomCancellationPolicies.Where(cancellationPolicy
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
                .Include(accommodation => accommodation.Location)
                .ThenInclude(location => location.Country)
                .Include(accommodation => accommodation.Images)
                .Where(accommodation => ratings.Contains(accommodation.Rating));
        }

        
        private Dictionary<Accommodation, List<AvailableRates>> GetAvailableRates(AvailabilityRequest availabilityRequest, List<Dictionary<RoomOccupationRequest, List<Room>>> groupedAvailableRooms, string languageCode)
        {
            var accommodationAvailableRatesStore = new Dictionary<Accommodation, List<AvailableRates>>();
            foreach (var accommodationGroupedRooms in groupedAvailableRooms)
            {
                if (accommodationGroupedRooms.Count != availabilityRequest.Rooms.Count)
                    continue;
                    
                var availableRateDetails = GetAvailableRateDetails(accommodationGroupedRooms);

                var rateDetailsCombinations = ListHelper.GetCombinations(availableRateDetails.Select(rateDetails => rateDetails.Value))
                    .Distinct()
                    .ToList();

                if (!rateDetailsCombinations.Any())
                    continue;

                var accommodation = accommodationGroupedRooms.First().Value.First().Accommodation;
                accommodationAvailableRatesStore.Add(accommodation, rateDetailsCombinations.Select(rateDetails => new AvailableRates {Rates = rateDetails}).ToList());
            }
                
            return accommodationAvailableRatesStore;

            
            Dictionary<RoomOccupationRequest, List<RateDetails>> GetAvailableRateDetails(Dictionary<RoomOccupationRequest, List<Room>> occupationRequestRoomsStore)
                => occupationRequestRoomsStore
                    .ToDictionary(occupationRequestRooms => occupationRequestRooms.Key, occupationRequestRooms => _rateAvailabilityService.GetAvailableRates(occupationRequestRooms.Key, occupationRequestRooms.Value.ToList(), availabilityRequest.CheckInDate, availabilityRequest.CheckOutDate, languageCode));
        }
        
        
        private readonly Dictionary<Accommodation,List<AvailableRates>> _emptyAvailableRates = new Dictionary<Accommodation, List<AvailableRates>>();
        private readonly IRateAvailabilityService _rateAvailabilityService;
        private readonly IRoomAvailabilityService _roomAvailabilityService;
        private readonly DirectContractsDbContext _dbContext;
    }
}