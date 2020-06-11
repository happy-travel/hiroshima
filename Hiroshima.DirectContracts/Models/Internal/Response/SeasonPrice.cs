using System;

namespace Hiroshima.DirectContracts.Models.Internal.Response
{
    public struct SeasonPrice
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal RatePrice { get; set; }
        public int Nights { get;set; }
        public decimal TotalPrice { get; set; }
    }
}