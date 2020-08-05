using System;
using System.Collections.Generic;

namespace HappyTravel.Hiroshima.DirectContracts.Models
{
    public readonly struct SeasonPriceDetails
    {
        public SeasonPriceDetails(DateTime startDate, DateTime endDate, decimal ratePrice, int numberOfNights, decimal totalPrice, List<string> details)
        {
            StartDate = startDate;
            EndDate = endDate;
            RatePrice = ratePrice;
            NumberOfNights = numberOfNights;
            TotalPrice = totalPrice;
            Details = details ?? new List<string>();
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
        /// Description
        /// </summary>
        public List<string> Details { get; }
    }
}