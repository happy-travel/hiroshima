using System;

namespace Hiroshima.DbData.Models
{
    public class Offer
    {
        public int Id { get; set; }
        public double DiscountPercent { get; set; }
        public int AccommodationId { get; set; }
        public DateTime ValidFromDate { get; set; }
        public DateTime ValidToDate { get; set; }
    }
}
