using System;

namespace HappyTravel.Hiroshima.DirectContracts.Models
{
    public readonly struct SeasonDailyPrice
    {
        public SeasonDailyPrice(DateTime fromDate, DateTime toDate, decimal price, decimal priceWithDiscount, string description)
        {
            Price = price;
            PriceWithDiscount = priceWithDiscount;
            FromDate = fromDate;
            ToDate = toDate;
            Description = description;
        }
        
        public DateTime FromDate { get; }
        public DateTime ToDate { get; }
        public decimal Price { get; }
        public decimal PriceWithDiscount { get; }
        public string Description { get; }
    }
}