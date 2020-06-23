using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HappyTravel.EdoContracts.Accommodations;
using Hiroshima.Common.Infrastructure.Extensions;
using Hiroshima.DbData.Models;
using Hiroshima.DbData.Models.Rooms;
using Hiroshima.DirectContracts.Models;
using Hiroshima.DirectContracts.Services.Availability;

namespace Hiroshima.DirectContracts.Services
{
    public class DcAvailabilityService : IDcAvailabilityService
    {
        public DcAvailabilityService(
            IDcAccommodationAvailabilityService dcAccommodationAvailabilityService,
            IDcRoomAvailabilityService dcRoomAvailabilityService,
            IDcRateAvailabilityService dcRateAvailabilityService,
            IDcCancellationPoliciesService dcCancellationPoliciesService)
        {
            _dcAccommodationAvailabilityService = dcAccommodationAvailabilityService;
            _dcRoomAvailabilityService = dcRoomAvailabilityService;
            _dcRateAvailabilityService = dcRateAvailabilityService;
            _dcCancellationPoliciesService = dcCancellationPoliciesService;
        }


        public async Task<List<AvailableAccommodation>> GetAvailable(AvailabilityRequest availabilityRequest, string languageCode)
        {
            var accommodations = await _dcAccommodationAvailabilityService.GetAccommodations(availabilityRequest, languageCode);
            
            var roomsGroupedByOccupationRequests =
                await _dcRoomAvailabilityService.GetGroupedRooms(accommodations, availabilityRequest, languageCode);
            
            var rooms = GetDistinctRooms(roomsGroupedByOccupationRequests);
            
            var availableRates = await _dcRateAvailabilityService.GetAvailableRates(rooms, availabilityRequest.CheckInDate.Date, availabilityRequest.CheckOutDate.Date, languageCode);

            return GetAvailableAccommodations(accommodations, roomsGroupedByOccupationRequests, availableRates);
        }


        List<AvailableAccommodation> GetAvailableAccommodations(List<AccommodationWithLocation> accommodations, List<RoomsGroupedByOccupation> roomsGroupedByOccupation, List<AvailableRate> availableRates)
        { 
            var availableAccommodations = new List<AvailableAccommodation>();

            var availableRatesDictionary = availableRates.ToDictionary(ar => ar.Room.Id);
            foreach (var groupedByOccupation in roomsGroupedByOccupation)
            {
                var availableAccommodation = new AvailableAccommodation();
                var roomCombinations = groupedByOccupation.SuitableRooms
                    .Select(sr => sr.Value)
                    .CartesianProduct();
                availableAccommodation.AccommodationWithLocation = groupedByOccupation.Accommodation;
                availableAccommodation.AvailableRateSets = GetAvailableRateSets(roomCombinations.ToList());
            }

            return availableAccommodations;
            
            
            List<AvailableRateSet> GetAvailableRateSets(List<IEnumerable<Room>> roomCombinations)
            {
                var availableRateSets = new List<AvailableRateSet>(roomCombinations.Count);
                foreach (var roomCombination in roomCombinations)
                {
                    var availableRateSet = new AvailableRateSet
                    {
                        AvailableRates = roomCombination
                            .Select(rc => availableRatesDictionary[rc.Id])
                            .ToList()
                    };
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


        private readonly IDcCancellationPoliciesService _dcCancellationPoliciesService;
        private readonly IDcRateAvailabilityService _dcRateAvailabilityService;
        private readonly IDcRoomAvailabilityService _dcRoomAvailabilityService;
        private readonly IDcAccommodationAvailabilityService _dcAccommodationAvailabilityService;
    }
}