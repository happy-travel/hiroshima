using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HappyTravel.EdoContracts.Accommodations.Enums;
using HappyTravel.EdoContracts.Accommodations.Internals;
using HappyTravel.EdoContracts.GeoData.Enums;
using HappyTravel.Hiroshima.Common.Constants;
using HappyTravel.Hiroshima.Common.Infrastructure.Utilities;
using HappyTravel.Hiroshima.Common.Models;
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


        public async Task<Dictionary<Accommodation, List<AvailableRates>>> Get(EdoContracts.Accommodations.AvailabilityRequest availabilityRequest, string languageCode)
        {
            var accommodations = await ExtractAvailabilityData();
  
            var groupedAvailableRooms = _roomAvailabilityService.GetGroupedAvailableRooms(accommodations, availabilityRequest.Rooms);

            return !DoAvailableRoomsMatchOccupancyRequest() 
                ? _emptyAvailableRates
                : GetAvailableRates();


            bool DoAvailableRoomsMatchOccupancyRequest() =>
                groupedAvailableRooms.Any(accommodationAvailableRooms=> accommodationAvailableRooms.Count == availabilityRequest.Rooms.Count);


            Dictionary<Accommodation, List<AvailableRates>> GetAvailableRates()
            {
                var accommodationAvailableRatesStore = new Dictionary<Accommodation, List<AvailableRates>>();
                foreach (var accommodationGroupedRooms in groupedAvailableRooms)
                {
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
            
            
            Task<List<Accommodation>> ExtractAvailabilityData()
            {
                var location = availabilityRequest.Location;
                var roomTypes = availabilityRequest.Rooms.Select(room => room.Type).ToList();
                
                switch (availabilityRequest.Location.Type)
                {
                    case LocationTypes.Location:
                    {
                        Expression<Func<Accommodation, bool>> expression = accommodation =>
                            accommodation.Location.Country.Name.RootElement.GetProperty(Languages.GetLanguageCode(Languages.DefaultLanguage))
                                .GetString() == location.Country &&
                            accommodation.Location.Locality.RootElement
                                .GetProperty(Languages.GetLanguageCode(Languages.DefaultLanguage))
                                .GetString() == location.Locality;

                        return GetAccommodationsWithAvailableRooms(availabilityRequest.CheckInDate, availabilityRequest.CheckOutDate, roomTypes, expression);
                    }
                    case LocationTypes.Accommodation:
                    {
                        Expression<Func<Accommodation, bool>> expression = accommodation =>
                            accommodation.Name.RootElement
                                .GetProperty(Languages.GetLanguageCode(Languages.DefaultLanguage))
                                .GetString() == location.Name;

                        return GetAccommodationsWithAvailableRooms(availabilityRequest.CheckInDate, availabilityRequest.CheckOutDate, roomTypes, expression);
                    }
                    case LocationTypes.Unknown:
                    case LocationTypes.Destination:
                    case LocationTypes.Landmark:
                        throw new NotImplementedException("Not implemented yet");
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }


        private async Task<List<Accommodation>> GetAccommodationsWithAvailableRooms(DateTime checkInDate, DateTime checkOutDate, List<RoomTypes> roomTypes, Expression<Func<Accommodation, bool>> expression)
        {
            return await _dbContext.Accommodations
                .Include(accommodation => accommodation.Rooms.Where(room => !room.RoomAvailabilityRestrictions.Any(availabilityRestrictions
                    => checkInDate <= availabilityRestrictions.ToDate && availabilityRestrictions.FromDate <= checkOutDate &&
                    availabilityRestrictions.Restriction == AvailabilityRestrictions.StopSale)))
                    .ThenInclude(room => room.RoomAllocationRequirements.Where(allocationRequirements => checkInDate <= allocationRequirements.SeasonRange.EndDate && allocationRequirements.SeasonRange.StartDate <= checkOutDate))
                        .ThenInclude(allocationRequirements => allocationRequirements.SeasonRange)
                .Include(accommodation => accommodation.Rooms)
                    .ThenInclude(room => room.RoomAvailabilityRestrictions.Where(availabilityRestrictions
                        => checkInDate <= availabilityRestrictions.ToDate && availabilityRestrictions.FromDate <= checkOutDate))
                .Include(accommodation => accommodation.Rooms)
                    .ThenInclude(room => room.RoomCancellationPolicies.Where(cancellationPolicy => cancellationPolicy.Season.SeasonRanges.Any(seasonRange => checkInDate <= seasonRange.EndDate && seasonRange.StartDate <= checkOutDate)))
                        .ThenInclude(cancellationPolicy => cancellationPolicy.Season)
                            .ThenInclude(season => season.SeasonRanges.Where(seasonRange => checkInDate <= seasonRange.EndDate && seasonRange.StartDate <= checkOutDate))
                .Include(accommodation => accommodation.Rooms)
                    .ThenInclude(room => room.RoomRates.Where(rate => roomTypes.Contains(rate.RoomType) &&  rate.Season.SeasonRanges.Any(seasonRange => checkInDate <= seasonRange.EndDate && seasonRange.StartDate <= checkOutDate)))
                        .ThenInclude(rate => rate.Season)
                            .ThenInclude(season => season.SeasonRanges.Where(seasonRange => checkInDate <= seasonRange.EndDate && seasonRange.StartDate <= checkOutDate))
                .Include(accommodation => accommodation.Rooms)
                    .ThenInclude(room => room.RoomPromotionalOffers.Where(promotionalOffer
                        => checkInDate <= promotionalOffer.ValidToDate && promotionalOffer.ValidFromDate <= checkOutDate))
                .Include(accommodation => accommodation.Location)
                    .ThenInclude(location => location.Country)
                .Include(accommodation => accommodation.Images)
                .Where(expression)
                .ToListAsync();
        }

        
        private readonly Dictionary<Accommodation,List<AvailableRates>> _emptyAvailableRates = new Dictionary<Accommodation, List<AvailableRates>>();
        private readonly IRateAvailabilityService _rateAvailabilityService;
        private readonly IRoomAvailabilityService _roomAvailabilityService;
        private readonly DirectContractsDbContext _dbContext;
    }
}