using System.Collections.Generic;
using Hiroshima.DbData.Models.Rooms;

namespace Hiroshima.DbData.Models.Rates
{
    public class ContractRate
    {
        public int Id { get; set; }
        public Room Room { get; set; }
        public int RoomId { get; set; }
        public string BoardBasisCode { get; set; }
        public string CurrencyCode { get; set; }
        public int ReleaseDays { get; set; }
        public decimal SeasonPrice { get; set; }
        public decimal ExtraPersonPrice { get; set; }
        public Season Season { get; set; }
        public int SeasonId { get; set; }
        public ICollection<Booking.Booking> Bookings { get; set; }
    }
}
