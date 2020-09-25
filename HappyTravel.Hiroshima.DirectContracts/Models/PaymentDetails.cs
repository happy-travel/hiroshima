using System.Collections.Generic;
using HappyTravel.Money.Enums;

namespace HappyTravel.Hiroshima.DirectContracts.Models
{
    public readonly struct PaymentDetails
    {
        public PaymentDetails(decimal totalPrice, List<SeasonPriceDetails> seasonPrices, Currencies currency, List<string> remarks)
        {
            TotalPrice = totalPrice;
            SeasonPrices = seasonPrices;
            Currency = currency;
            Remarks = remarks ?? new List<string>();
        }


        public decimal TotalPrice { get; }
        public List<SeasonPriceDetails> SeasonPrices { get; }
        public Currencies Currency { get; }
        public List<string> Remarks { get; }
    }
}