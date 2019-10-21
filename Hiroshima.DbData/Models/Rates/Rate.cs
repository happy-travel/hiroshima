using System.Collections.Generic;
using Hiroshima.DbData.Models.Rooms;

namespace Hiroshima.DbData.Models.Rates
{
    public class Rate
    {
        public int Id { get; set; }
        public Room Room { get; set; }
        public int RoomId { get; set; }
        public string BoardBasisCode { get; set; }
        public BoardBasis BoardBasis { get; set; }
        public string CurrencyCode { get; set; }
        public Currency Currency { get; set; }
        public int ReleaseDays { get; set; }
        public decimal Price { get; set; }
        public decimal ExtraPersonPrice { get; set; }
        public Season Season { get; set; }
        public int SeasonId { get; set; }
        public IEnumerable<Booking.Booking> Bookings { get; set; }
    }
}
