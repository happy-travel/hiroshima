using System.Collections.Generic;

namespace Hiroshima.DbData.Models
{
    public class Rate
    {
        public int Id { get; set; }
        public Room Room { get; set; }
        public int RoomId { get; set; }
        public string BoardBasisCode { get; set; }
        public BoardBasis BoardBasis { get; set; }
        public int ReleaseDays { get; set; }
        public Price Price { get; set; }
        public Season Season { get; set; }
        public int SeasonId { get; set; }
        public IEnumerable<Booking> Bookings { get; set; }

    }
}
