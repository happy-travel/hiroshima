using System.Collections.Generic;
using HappyTravel.EdoContracts.Accommodations.Internals;
using Hiroshima.DbData.Models;
using Hiroshima.DbData.Models.Rooms;

namespace Hiroshima.DirectContracts.Models.Internal
{
    public class RoomsGroupedByOccupationRequest
    {
        public AccommodationData Accommodation { get; set; }
        public Dictionary<RoomOccupationRequest, List<Room>> SuitableRooms { get; set; }
    }

}