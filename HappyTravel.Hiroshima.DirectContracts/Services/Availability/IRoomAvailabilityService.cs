using System.Collections.Generic;
using HappyTravel.EdoContracts.Accommodations.Internals;
using HappyTravel.Hiroshima.Common.Models.Accommodations;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms;

namespace HappyTravel.Hiroshima.DirectContracts.Services.Availability
{
    public interface IRoomAvailabilityService
    {
        List<Dictionary<RoomOccupationRequest, List<Room>>> GetGroupedAvailableRooms(List<Accommodation> accommodations, List<RoomOccupationRequest> occupationRequest);
    }
}