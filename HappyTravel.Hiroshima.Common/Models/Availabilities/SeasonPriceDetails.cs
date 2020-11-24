using System;
using System.Collections.Generic;
using HappyTravel.EdoContracts.Extensions;
using HappyTravel.EdoContracts.General;
using HappyTravel.Hiroshima.Common.Infrastructure.Utilities;
using HappyTravel.Money.Models;
using Newtonsoft.Json;

namespace HappyTravel.Hiroshima.Common.Models.Availabilities
{
    public readonly struct SeasonPriceDetails
    {
        public SeasonPriceDetails(DateTime startDate, DateTime endDate, MoneyAmount rateAmount, int numberOfNights, MoneyAmount totalAmount, MoneyAmount totalAmountWithDiscount, Discount discount, List<SeasonDailyPrice> dailyPrices)
        {
            StartDate = startDate;
            EndDate = endDate;
            RateAmount = rateAmount;
            NumberOfNights = numberOfNights;
            TotalAmount = totalAmount;
            TotalAmountWithDiscount = totalAmountWithDiscount;
            Discount = discount;
            DailyPrices = dailyPrices;
        }

        
        public override bool Equals(object? obj) => obj is SeasonDailyPrice other && Equals(other);
        
        
        public bool Equals(in SeasonPriceDetails other)
            => (StartDate, EndDate, RateAmount, NumberOfNights, TotalAmount, TotalAmountWithDiscount, Discount.Description, Discount.Percent).Equals((StartDate, EndDate, RateAmount, NumberOfNights, TotalAmount, TotalAmountWithDiscount, Discount.Description, Discount.Percent))
                && DailyPrices.SafeSequenceEqual(other.DailyPrices);

        
        public override int GetHashCode() => Hash.Aggregate<SeasonDailyPrice>(DailyPrices, HashCode.Combine(StartDate, EndDate, RateAmount, NumberOfNights, TotalAmount, TotalAmountWithDiscount, Discount.Description, Discount.Percent));
        

        /// <summary>
        /// Season start date
        /// </summary>
        public DateTime StartDate { get; }
        
        /// <summary>
        /// Season end date
        /// </summary>
        public DateTime EndDate { get; }
        
        /// <summary>
        /// Base rate money amount per night
        /// </summary>
        public MoneyAmount RateAmount { get; }
        
        /// <summary>
        /// Number of nights in the season
        /// </summary>
        public int NumberOfNights { get; }
        
        /// <summary>
        /// TotalAmount = NumberOfNights * RateAmount
        /// </summary>
        public MoneyAmount TotalAmount { get; }
        
        /// <summary>
        /// Total money amount with the discount 
        /// </summary>
        public MoneyAmount TotalAmountWithDiscount { get; }
        
        /// <summary>
        /// Total discount 
        /// </summary>
        public Discount Discount { get; }
        
        /// <summary>
        /// Daily prices
        /// </summary>
        [JsonIgnore]
        public List<SeasonDailyPrice> DailyPrices { get; }
    }
}