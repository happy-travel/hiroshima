using System;
using System.Collections.Generic;
using HappyTravel.EdoContracts.Accommodations.Internals;

namespace HappyTravel.Hiroshima.DirectContracts.Models
{
    public readonly struct SeasonPriceDetails
    {
        public SeasonPriceDetails(DateTime startDate, DateTime endDate, decimal ratePrice, int numberOfNights, decimal totalPrice, List<double> appliedDiscounts, List<string> remarks)
        {
            StartDate = startDate;
            EndDate = endDate;
            RatePrice = ratePrice;
            NumberOfNights = numberOfNights;
            TotalPrice = totalPrice;
            AppliedDiscounts = appliedDiscounts ?? new List<double>();
            Remarks = remarks ?? new List<string>();
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
        /// Remarks
        /// </summary>
        public List<string> Remarks { get; }
        
        
        /// <summary>
        /// Discount percent applied to a rate price 
        /// </summary>
        public List<double> AppliedDiscounts { get; }
    }
}