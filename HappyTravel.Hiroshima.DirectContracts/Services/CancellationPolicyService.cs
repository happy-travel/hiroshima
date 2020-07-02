using System;
using System.Collections.Generic;
using System.Linq;
using HappyTravel.Hiroshima.DbData.Models.Room.CancellationPolicies;
using HappyTravel.Hiroshima.DirectContracts.Models;
using HappyTravel.Money.Helpers;

namespace HappyTravel.Hiroshima.DirectContracts.Services
{
    public class CancellationPolicyService: ICancellationPolicyService
    {
        public List<CancellationPolicyDetails> Get(RoomCancellationPolicy roomCancellationPolicy, DateTime checkInDate, PaymentDetails paymentDetails)
        {
            var cancellationPolicyData = roomCancellationPolicy.Details;
            var cancellationPolicyDetails = new List<CancellationPolicyDetails>(cancellationPolicyData.Count);
            
            foreach (var cancellationPolicyDataItem in cancellationPolicyData)
            {
                var cancellationDetails = new CancellationPolicyDetails
                {
                    StartDate = checkInDate.Date.AddDays(-cancellationPolicyDataItem.DaysInterval.ToDays),
                    EndDate = checkInDate.Date.AddDays(-cancellationPolicyDataItem.DaysInterval.FromDays),
                    Price = cancellationPolicyDataItem.PenaltyType == CancellationPenaltyTypes.Percent
                        ? CalculatePercentPenaltyPrice(cancellationPolicyDataItem.PenaltyCharge, paymentDetails)
                        : CalculateNightsPenaltyPrice(cancellationPolicyDataItem.PenaltyCharge, paymentDetails,
                            checkInDate)
                };
                cancellationPolicyDetails.Add(cancellationDetails);
            }

            return cancellationPolicyDetails;
        }
        

        private decimal CalculatePercentPenaltyPrice(double percentToCharge, PaymentDetails paymentDetails)
        => MoneyRounder.Ceil(paymentDetails.TotalPrice * Convert.ToDecimal(percentToCharge) / 100, paymentDetails.Currency);
        
        
        private decimal CalculateNightsPenaltyPrice(double nightsToCharge, PaymentDetails paymentDetails, DateTime checkInDate)
        {
            var penaltyPrice = 0m;
            
            for (var i = 0; i < nightsToCharge; i++)
                penaltyPrice += GetSeasonPrice(checkInDate.AddDays(i));

            return penaltyPrice;

            decimal GetSeasonPrice(DateTime date)
            {
                foreach (var seasonPrice in paymentDetails.SeasonPrices)
                {
                    if (seasonPrice.StartDate <= date && date <= seasonPrice.EndDate)
                        return seasonPrice.RatePrice;
                }
                
                //TODO Add to log this strange case
                return paymentDetails.SeasonPrices.Max(s => s.RatePrice);
            }
        }
    }
}