using System;
using System.Collections.Generic;

namespace HappyTravel.Hiroshima.DirectContracts.Models
{
    public readonly struct SeasonPriceDetails
    {
        public SeasonPriceDetails(DateTime startDate, DateTime endDate, decimal ratePrice, int numberOfNights, decimal totalPrice, decimal totalPriceWithDiscount, decimal totalDiscountPercent, List<SeasonDailyPrice> dailyPrices)
        {
            StartDate = startDate;
            EndDate = endDate;
            RatePrice = ratePrice;
            NumberOfNights = numberOfNights;
            TotalPrice = totalPrice;
            TotalPriceWithDiscount = totalPriceWithDiscount;
            TotalDiscountPercent = totalDiscountPercent;
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
        /// Base rate price per night
        /// </summary>
        public decimal RatePrice { get; }
        
        /// <summary>
        /// Number of nights in the season
        /// </summary>
        public int NumberOfNights { get; }
        
        /// <summary>
        /// TotalPrice = NumberOfNights * RatePrice
        /// </summary>
        public decimal TotalPrice { get; }
        
        
        /// <summary>
        /// Total price with discount 
        /// </summary>
        public decimal TotalPriceWithDiscount { get; }
        
        /// <summary>
        /// Total discount percent 
        /// </summary>
        public decimal TotalDiscountPercent { get; }
        
        /// <summary>
        /// Daily prices
        /// </summary>
        public List<SeasonDailyPrice> DailyPrices { get; }
    }
}