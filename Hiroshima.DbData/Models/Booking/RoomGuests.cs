using System.Collections.Generic;

namespace Hiroshima.DbData.Models.Booking
{
    public class RoomGuests
    {
        public List<Guest> Guests { get; set; }
        public int RoomRateId { get; set; }
    }
}