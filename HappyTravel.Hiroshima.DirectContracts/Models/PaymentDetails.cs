using System.Collections.Generic;
using HappyTravel.Money.Enums;

namespace HappyTravel.Hiroshima.DirectContracts.Models
{
    public readonly struct PaymentDetails
    {
        public PaymentDetails(decimal totalPrice, List<decimal> dailyPrices, List<SeasonPriceDetails> seasonPrices, Currencies currency, List<string> details)
        {
            TotalPrice = totalPrice;
            DailyPrices = dailyPrices;
            SeasonPrices = seasonPrices;
            Currency = currency;
            Details = details ?? new List<string>();
        }


        public decimal TotalPrice { get; }
        public List<decimal> DailyPrices { get; }
        public List<SeasonPriceDetails> SeasonPrices { get; }
        public Currencies Currency { get; }
        public List<string> Details { get; }
    }
}