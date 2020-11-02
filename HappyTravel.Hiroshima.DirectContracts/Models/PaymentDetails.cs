using System.Collections.Generic;
using HappyTravel.EdoContracts.General;
using HappyTravel.Money.Models;

namespace HappyTravel.Hiroshima.DirectContracts.Models
{
    public readonly struct PaymentDetails
    {
        public PaymentDetails(MoneyAmount totalAmount, Discount discount, List<SeasonPriceDetails> seasonPrices, List<string> remarks)
        {
            TotalAmount = totalAmount;
            Discount = discount;
            SeasonPrices = seasonPrices;
            Remarks = remarks ?? new List<string>();
        }


        public MoneyAmount TotalAmount { get; }
        public Discount Discount { get; }
        public List<SeasonPriceDetails> SeasonPrices { get; }
        public List<string> Remarks { get; }
    }
}