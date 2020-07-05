using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HappyTravel.EdoContracts.Accommodations;
using HappyTravel.EdoContracts.GeoData;
using HappyTravel.EdoContracts.GeoData.Enums;
using HappyTravel.Hiroshima.Common.Infrastructure.Extensions;
using HappyTravel.Hiroshima.Data.Models.Rooms;
using HappyTravel.Hiroshima.DirectContracts.Models;
using AccommodationDetails = HappyTravel.Hiroshima.Data.Models.AccommodationDetails;
using AvailabilityDetails = HappyTravel.Hiroshima.DirectContracts.Models.AvailabilityDetails;

namespace HappyTravel.Hiroshima.DirectContracts.Services.Availability
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
            
            var roomsGroupedByOccupationRequest =
                await _roomAvailabilityService.GetGroupedRooms(accommodations, availabilityRequest, languageCode);
            
            var rooms = GetDistinctRooms(roomsGroupedByOccupationRequest);
            
            var availableRates = await _rateAvailabilityService.GetAvailableRates(rooms, availabilityRequest.CheckInDate.Date, availabilityRequest.CheckOutDate.Date, languageCode);
            
            var availabilityDetails = ExtractAvailabilityDetails(roomsGroupedByOccupationRequest, availableRates);

            return availabilityDetails;
            
            
            Task<List<AccommodationDetails>> GetAccommodations(Location location, string languageCode)
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
        }


        private List<AvailabilityDetails> ExtractAvailabilityDetails(List<RoomsGroupedByOccupation> roomsGroupedByOccupation, List<RateOffer> availableRates)
        { 
            var availabilityDetails = new List<AvailabilityDetails>();

            var availableRatesDictionary = availableRates.ToDictionary(ar => ar.Room.Id);
            foreach (var groupedByOccupation in roomsGroupedByOccupation)
            {
                var availableAccommodation = new AvailabilityDetails();
                var roomCombinations = groupedByOccupation.SuitableRooms
                    .Select(sr => sr.Value)
                    .CartesianProduct();
                availableAccommodation.AccommodationDetails = groupedByOccupation.Accommodation;
                availableAccommodation.AvailableRateOffers = GetAvailableRates(roomCombinations.ToList());
                availabilityDetails.Add(availableAccommodation);
            }

            return availabilityDetails;
            
            
            List<List<RateOffer>> GetAvailableRates(List<IEnumerable<Room>> roomCombinations)
            {
                var availableRates = new List<List<RateOffer>>(roomCombinations.Count);
                foreach (var roomCombination in roomCombinations)
                {
                    var rateOffers = new List<RateOffer>(roomCombination
                        .Select(rc => availableRatesDictionary[rc.Id])
                        .ToList());
                    availableRates.Add(rateOffers);
                }

                return availableRates;
            }
        }

        
        private List<Room> GetDistinctRooms(List<RoomsGroupedByOccupation> rooms) => rooms
            .SelectMany(rg => rg.SuitableRooms.Values)
            .SelectMany(rl => rl)
            .GroupBy(r => r.Id)
            .Select(grp => grp.FirstOrDefault()).ToList();


        private readonly IRateAvailabilityService _rateAvailabilityService;
        private readonly IRoomAvailabilityService _roomAvailabilityService;
        private readonly IAvailabilityRepository _availabilityRepository;
    }
}