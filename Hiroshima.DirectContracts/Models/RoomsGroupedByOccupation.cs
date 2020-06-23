using System.Collections.Generic;
using HappyTravel.EdoContracts.Accommodations.Internals;
using Hiroshima.DbData.Models;
using Hiroshima.DbData.Models.Rooms;

namespace Hiroshima.DirectContracts.Models
{
    public struct RoomsGroupedByOccupation
    {
        public AccommodationWithLocation Accommodation { get; set; }
        public Dictionary<RoomOccupationRequest, List<Room>> SuitableRooms { get; set; }
    }
}