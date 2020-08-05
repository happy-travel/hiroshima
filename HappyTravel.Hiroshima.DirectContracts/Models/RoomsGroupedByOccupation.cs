using System.Collections.Generic;
using HappyTravel.EdoContracts.Accommodations.Internals;
using HappyTravel.Hiroshima.Data.Models.Rooms;

namespace HappyTravel.Hiroshima.DirectContracts.Models
{
    public readonly struct RoomsGroupedByOccupation
    {
        public RoomsGroupedByOccupation(AccommodationDetails accommodation, Dictionary<RoomOccupationRequest, List<Room>> suitableRooms)
        {
            Accommodation = accommodation;
            SuitableRooms = suitableRooms ?? new Dictionary<RoomOccupationRequest, List<Room>>();
        }


        public AccommodationDetails Accommodation { get; }
        public Dictionary<RoomOccupationRequest, List<Room>> SuitableRooms { get; }
    }
}