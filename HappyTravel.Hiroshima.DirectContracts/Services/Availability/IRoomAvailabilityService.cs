using System.Collections.Generic;
using HappyTravel.EdoContracts.Accommodations;
using HappyTravel.EdoContracts.Accommodations.Internals;
using HappyTravel.Hiroshima.Common.Models.Accommodations;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms;
using Accommodation = HappyTravel.Hiroshima.Common.Models.Accommodations.Accommodation;

namespace HappyTravel.Hiroshima.DirectContracts.Services.Availability
{
    public interface IRoomAvailabilityService
    {
        List<Dictionary<RoomOccupationRequest, List<Room>>> GetGroupedAvailableRooms(AvailabilityRequest availabilityRequest,
            List<Accommodation> accommodations, List<RoomOccupationRequest> occupationRequest);
    }
}