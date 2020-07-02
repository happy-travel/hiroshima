using System.Collections.Generic;
using HappyTravel.EdoContracts.Accommodations.Internals;
using Hiroshima.DbData.Models;
using Hiroshima.DbData.Models.Room;

namespace Hiroshima.DirectContracts.Models
{
    public struct RoomsGroupedByOccupation
    {
        public AccommodationDetails Accommodation { get; set; }
        public Dictionary<RoomOccupationRequest, List<Room>> SuitableRooms { get; set; }
    }
}