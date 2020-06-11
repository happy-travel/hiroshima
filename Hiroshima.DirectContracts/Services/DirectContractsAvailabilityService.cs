using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HappyTravel.EdoContracts.Accommodations;
using Hiroshima.Common.Models.Enums;
using Hiroshima.DbData.Models.Rooms;
using Hiroshima.DirectContracts.Models.Internal;
using Hiroshima.DirectContracts.Services.Availability;

namespace Hiroshima.DirectContracts.Services
{
    public class DirectContractsAvailabilityService : IDirectContractsAvailabilityService
    {
        public DirectContractsAvailabilityService(
            IDirectContractsAccommodationAvailabilityService dcAccommodationAvailabilityService,
            IDirectContractsRoomAvailabilityService dcRoomAvailabilityService,
            IDirectContractsRateAvailabilityService dcRateAvailabilityService,
            IDirectContractsCancellationPoliciesService dcCancellationPoliciesService)
        {
            _dcAccommodationAvailabilityService = dcAccommodationAvailabilityService;
            _dcRoomAvailabilityService = dcRoomAvailabilityService;
            _dcRateAvailabilityService = dcRateAvailabilityService;
            _dcCancellationPoliciesService = dcCancellationPoliciesService;
        }


        public async Task<AvailabilityDetails> GetAvailable(AvailabilityRequest availabilityRequest,
            Languages language)
        {
            var checkInDate = availabilityRequest.CheckInDate.Date;
            var checkOutDate = availabilityRequest.CheckOutDate.Date;

            var location = availabilityRequest.Location;
            var localityName = location.Name;
            var countryName = location.Country;
            var locationName = location.Name;

            var accommodations = await _dcAccommodationAvailabilityService.GetAccommodations(availabilityRequest);

            var roomsGroupedByOccupationRequests =
                await _dcRoomAvailabilityService.GetGroupedRooms(accommodations, availabilityRequest);
            
            var rooms = GetDistinctRooms(roomsGroupedByOccupationRequests);
            
            await _dcRateAvailabilityService.GetAvailableRates(rooms, checkInDate, checkOutDate);
            /*var promotionalOfferRates =
                _dcPromotionalOfferAvailabilityService.GetPromotionalOfferRates(rates, checkInDate);
*/
            throw new NotImplementedException("Return empty");
            /*
                   
                   if (!availabilityRequest.Location.Coordinates.Equals(default))
                   {
                       if (string.IsNullOrWhiteSpace(availabilityRequest.Location.Name))
                           rawAvailabilityData = _availabilityRequestsService.GetAvailability(
                               checkInDate,
                               checkOutDate,
                               _geometryFactory.CreatePoint(new Coordinate(location.Coordinates.Longitude, location.Coordinates.Latitude)),
                               Convert.ToDouble(availabilityRequest.Location.Distance));
                       else
                           rawAvailabilityData = _availabilityRequestsService.GetAvailability(
                               checkInDate,
                               checkOutDate,
                               availabilityRequest.Location.Name,
                               _geometryFactory.CreatePoint(new Coordinate(location.Coordinates.Longitude, location.Coordinates.Latitude)),
                               availabilityRequest.Location.Distance);
                   }
                   else
                   {
                       if (!string.IsNullOrWhiteSpace(location.Name) ||
                           !string.IsNullOrWhiteSpace(location.Locality))
                           rawAvailabilityData = _availabilityRequestsService.GetAvailability(
                               checkInDate,
                               checkOutDate,
                               availabilityRequest.Location.Name,
                               availabilityRequest.Location.Locality,
                               availabilityRequest.Location.Country);
                   }
           
                   var rawAvailabilityItems = await rawAvailabilityData.ToListAsync();
           
                   if (!rawAvailabilityItems.Any())
                       return _availabilityResponseService.GetEmptyAvailabilityDetails(checkInDate, checkOutDate);
           
                   var filteredRawAvailabilityItems = _rawAvailabilityDataFilter.ByRoomDetails(rawAvailabilityItems, availabilityRequest.RoomDetails);
           
                   return _availabilityResponseService.GetAvailabilityDetails(checkInDate, checkOutDate, filteredRawAvailabilityItems,
                       languages); */
        }


        private List<Room> GetDistinctRooms(List<RoomsGroupedByOccupationRequest> rooms) => rooms
            .SelectMany(rg => rg.SuitableRooms.Values)
            .SelectMany(rl => rl)
            .GroupBy(r => r.Id)
            .Select(grp => grp.FirstOrDefault()).ToList();


        private readonly IDirectContractsCancellationPoliciesService _dcCancellationPoliciesService;
        private readonly IDirectContractsRateAvailabilityService _dcRateAvailabilityService;
        private readonly IDirectContractsRoomAvailabilityService _dcRoomAvailabilityService;
        private readonly IDirectContractsAccommodationAvailabilityService _dcAccommodationAvailabilityService;
    }
}