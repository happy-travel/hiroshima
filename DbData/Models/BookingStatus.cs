using System;
using System.Collections.Generic;
using System.Text;

namespace Hiroshima.DbData.Models
{
    public class BookingStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Booking> Bookings { get; set; }
    }
}
