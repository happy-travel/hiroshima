using System.Collections.Generic;
using HappyTravel.Money.Enums;

namespace HappyTravel.Hiroshima.DirectContracts.Models
{
    public readonly struct PaymentDetails
    {
        public PaymentDetails(decimal priceTotal, double discountPercent, List<SeasonPriceDetails> seasonPrices, Currencies currency, List<string> remarks)
        {
            PriceTotal = priceTotal;
            DiscountPercent = discountPercent;
            SeasonPrices = seasonPrices;
            Currency = currency;
            Remarks = remarks ?? new List<string>();
        }


        public decimal PriceTotal { get; }
        public double DiscountPercent { get; }
        public List<SeasonPriceDetails> SeasonPrices { get; }
        public Currencies Currency { get; }
        public List<string> Remarks { get; }
    }
}