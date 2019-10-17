using System.Collections.Generic;

namespace Hiroshima.DbData.Models.Booking
{
    public class BookingStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Booking> Bookings { get; set; }
    }
}
