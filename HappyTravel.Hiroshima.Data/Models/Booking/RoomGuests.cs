using System.Collections.Generic;

namespace HappyTravel.Hiroshima.Data.Models.Booking
{
    public class RoomGuests
    {
        public List<Guest> Guests { get; set; }
        public int RoomRateId { get; set; }
    }
}