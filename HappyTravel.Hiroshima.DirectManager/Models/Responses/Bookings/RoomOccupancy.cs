using System.Collections.Generic;
using HappyTravel.EdoContracts.Accommodations.Enums;

namespace HappyTravel.Hiroshima.DirectManager.Models.Responses.Bookings
{
    public readonly struct RoomOccupancy
    {
        public RoomOccupancy(RoomTypes roomType, List<Pax> passengers)
        {
            RoomType = roomType;
            Passengers = passengers;
        }
        
        
        public RoomTypes RoomType { get; } 
        public List<Pax> Passengers { get; }
    }
}