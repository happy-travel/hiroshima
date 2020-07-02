using System;

namespace HappyTravel.Hiroshima.DirectContracts.Models
{
    public struct SeasonPriceDetails
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal RatePrice { get; set; }
        public int NumberOfNights { get; set; }
        public decimal TotalPrice { get; set; }
    }
}