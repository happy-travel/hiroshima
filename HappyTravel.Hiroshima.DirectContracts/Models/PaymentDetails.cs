using System;
using System.Collections.Generic;
using HappyTravel.EdoContracts.Extensions;
using HappyTravel.EdoContracts.General;
using HappyTravel.Hiroshima.Common.Infrastructure.Utilities;
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
            Remarks = remarks;
        }
        
        
        public override bool Equals(object? obj) => obj is PaymentDetails other && Equals(other);
        
        
        public bool Equals(in PaymentDetails other)
            => (TotalAmount, Discount.Percent, Discount.Description).Equals((other.TotalAmount, other.Discount.Percent, other.Discount.Description))
                && Remarks.SafeSequenceEqual(other.Remarks) && SeasonPrices.SafeSequenceEqual(other.SeasonPrices);

        
        public override int GetHashCode() => Hash.Aggregate<SeasonPriceDetails>(SeasonPrices, Hash.Aggregate<string>(Remarks, HashCode.Combine(TotalAmount, Discount.Percent, Discount.Description)));

        
        public MoneyAmount TotalAmount { get; }
        public Discount Discount { get; }
        public List<SeasonPriceDetails> SeasonPrices { get; }
        public List<string> Remarks { get; }
    }
}