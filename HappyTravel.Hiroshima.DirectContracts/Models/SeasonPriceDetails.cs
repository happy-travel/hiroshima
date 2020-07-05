using System;
using System.Collections.Generic;

namespace HappyTravel.Hiroshima.DirectContracts.Models
{
    public struct SeasonPriceDetails
    {
        /// <summary>
        /// Season start date
        /// </summary>
        public DateTime StartDate { get; set; }
        
        /// <summary>
        /// Season end date
        /// </summary>
        public DateTime EndDate { get; set; }
        
        /// <summary>
        /// Base rate price per night
        /// </summary>
        public decimal RatePrice { get; set; }
        
        /// <summary>
        /// Number of nights in the season
        /// </summary>
        public int NumberOfNights { get; set; }
        
        /// <summary>
        /// TotalPrice = NumberOfNights * RatePrice
        /// </summary>
        public decimal TotalPrice { get; set; }
        
        /// <summary>
        /// Description
        /// </summary>
        public List<string> Details { get; set; }
    }
}