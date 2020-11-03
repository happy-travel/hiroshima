using System;
using System.Collections.Generic;
using HappyTravel.EdoContracts.General;
using HappyTravel.Money.Models;

namespace HappyTravel.Hiroshima.DirectContracts.Models
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
        public List<SeasonDailyPrice> DailyPrices { get; }
    }
}