using System;
using HappyTravel.EdoContracts.General;
using HappyTravel.Money.Models;

namespace HappyTravel.Hiroshima.Common.Models.Availabilities
{
    public readonly struct SeasonDailyPrice
    {
        public SeasonDailyPrice(DateTime fromDate, DateTime toDate, MoneyAmount dailyAmount, MoneyAmount dailyAmountWithDiscount, Discount discount)
        {
            DailyAmount = dailyAmount;
            DailyAmountWithDiscount = dailyAmountWithDiscount;
            FromDate = fromDate;
            ToDate = toDate;
            Discount = discount;
        }

        
        public override bool Equals(object? obj) => obj is SeasonDailyPrice other && Equals(other);

        
        public bool Equals(in SeasonDailyPrice other)
            => (DailyAmount, DailyAmountWithDiscount, FromDate, ToDate, Discount.Description, Discount.Percent).Equals((other.DailyAmount, other.DailyAmountWithDiscount,
                other.FromDate, other.ToDate, other.Discount.Description, other.Discount.Percent));


        public override int GetHashCode() => HashCode.Combine(DailyAmount, DailyAmountWithDiscount, FromDate, ToDate, Discount.Description, Discount.Percent);

        
        public DateTime FromDate { get; }
        public DateTime ToDate { get; }
        public MoneyAmount DailyAmount { get; }
        public MoneyAmount DailyAmountWithDiscount { get; }
        public Discount Discount { get; }
    }
}