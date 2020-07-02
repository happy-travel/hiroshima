using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HappyTravel.EdoContracts.Accommodations;
using HappyTravel.EdoContracts.GeoData;
using HappyTravel.EdoContracts.GeoData.Enums;
using Hiroshima.Common.Infrastructure.Extensions;
using Hiroshima.DbData.Models;
using Hiroshima.DbData.Models.Room;
using Hiroshima.DirectContracts.Models;
using AccommodationDetails = Hiroshima.DbData.Models.AccommodationDetails;
using AvailabilityDetails = Hiroshima.DirectContracts.Models.AvailabilityDetails;

namespace Hiroshima.DirectContracts.Services.Availability
{
    public class AvailabilityService : IAvailabilityService
    {
        public AvailabilityService(
            IRoomAvailabilityService roomAvailabilityService,
            IRateAvailabilityService rateAvailabilityService,
            IAvailabilityRepository availabilityRepository)
        {
            _roomAvailabilityService = roomAvailabilityService;
            _rateAvailabilityService = rateAvailabilityService;
            _availabilityRepository = availabilityRepository;
        }


        public async Task<List<AvailabilityDetails>> Get(AvailabilityRequest availabilityRequest, string languageCode)
        {
            var accommodations = await GetAccommodations(availabilityRequest.Location, languageCode);
            
            var roomsGroupedByOccupationRequests =
                await _roomAvailabilityService.GetGroupedRooms(accommodations, availabilityRequest, languageCode);
            
            var rooms = GetDistinctRooms(roomsGroupedByOccupationRequests);
            
            var availableRates = await _rateAvailabilityService.GetAvailableRates(rooms, availabilityRequest.CheckInDate.Date, availabilityRequest.CheckOutDate.Date, languageCode);
            
            var availableAccommodations = GetAvailabilityDetails(accommodations, roomsGroupedByOccupationRequests, availableRates);

            return availableAccommodations;
        }


        private List<AvailabilityDetails> GetAvailabilityDetails(List<AccommodationDetails> accommodations, List<RoomsGroupedByOccupation> roomsGroupedByOccupation, List<RateOffer> availableRates)
        { 
            var availableAccommodations = new List<AvailabilityDetails>();

            var availableRatesDictionary = availableRates.ToDictionary(ar => ar.Room.Id);
            foreach (var groupedByOccupation in roomsGroupedByOccupation)
            {
                var availableAccommodation = new AvailabilityDetails();
                var roomCombinations = groupedByOccupation.SuitableRooms
                    .Select(sr => sr.Value)
                    .CartesianProduct();
                availableAccommodation.AccommodationDetails = groupedByOccupation.Accommodation;
                availableAccommodation.AvailableRateOffers = GetAvailableRates(roomCombinations.ToList());
                availableAccommodations.Add(availableAccommodation);
            }

            return availableAccommodations;
            
            
            List<List<RateOffer>> GetAvailableRates(List<IEnumerable<Room>> roomCombinations)
            {
                var availableRateSets = new List<List<RateOffer>>(roomCombinations.Count);
                foreach (var roomCombination in roomCombinations)
                {
                    var availableRateSet = new List<RateOffer>(roomCombination
                        .Select(rc => availableRatesDictionary[rc.Id])
                        .ToList());
                    availableRateSets.Add(availableRateSet);
                }

                return availableRateSets;
            }
        }

        
        private List<Room> GetDistinctRooms(List<RoomsGroupedByOccupation> rooms) => rooms
            .SelectMany(rg => rg.SuitableRooms.Values)
            .SelectMany(rl => rl)
            .GroupBy(r => r.Id)
            .Select(grp => grp.FirstOrDefault()).ToList();

        
        private Task<List<AccommodationDetails>> GetAccommodations(Location location, string languageCode)
        {
            switch (location.Type)
            {
                case LocationTypes.Location:
                    return _availabilityRepository.GetAccommodations(location.Country, location.Locality, languageCode);
                case LocationTypes.Accommodation:
                    return _availabilityRepository.GetAccommodations(location.Name, languageCode);
            }

            return Task.FromResult(new List<AccommodationDetails>());
        }
        
        
        private readonly IRateAvailabilityService _rateAvailabilityService;
        private readonly IRoomAvailabilityService _roomAvailabilityService;
        private readonly IAvailabilityRepository _availabilityRepository;
    }
}