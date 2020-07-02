using System.Collections.Generic;
using HappyTravel.Money.Enums;

namespace Hiroshima.DirectContracts.Models
{
    public struct PaymentDetails
    {
        public decimal TotalPrice { get; set; }
        public List<decimal> DailyPrices { get; set; }
        public List<SeasonPriceDetails> SeasonPrices { get; set; }
        public Currencies Currency { get; set; }
        public List<string>? Details { get; set; }
    }
}