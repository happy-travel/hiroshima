using System;
using HappyTravel.EdoContracts.General;
using HappyTravel.Money.Models;

namespace HappyTravel.Hiroshima.DirectContracts.Models
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
        
        
        public DateTime FromDate { get; }
        public DateTime ToDate { get; }
        public MoneyAmount DailyAmount { get; }
        public MoneyAmount DailyAmountWithDiscount { get; }
        public Discount Discount { get; }
    }
}