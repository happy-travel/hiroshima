using System.Collections.Generic;
using HappyTravel.Money.Enums;

namespace HappyTravel.Hiroshima.DirectContracts.Models
{
    public readonly struct PaymentDetails
    {
        public PaymentDetails(decimal totalPrice, decimal discountPercent, List<SeasonPriceDetails> seasonPrices, Currencies currency, List<string> remarks)
        {
            TotalPrice = totalPrice;
            DiscountPercent = discountPercent;
            SeasonPrices = seasonPrices;
            Currency = currency;
            Remarks = remarks ?? new List<string>();
        }


        public decimal TotalPrice { get; }
        public decimal DiscountPercent { get; }
        public List<SeasonPriceDetails> SeasonPrices { get; }
        public Currencies Currency { get; }
        public List<string> Remarks { get; }
    }
}