using System;
using System.Collections.Generic;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.CancellationPolicies;
using HappyTravel.Hiroshima.DirectContracts.Models;
using HappyTravel.Money.Helpers;

namespace HappyTravel.Hiroshima.DirectContracts.Services
{
    public class CancellationPolicyService: ICancellationPolicyService
    {
        public List<CancellationPolicyDetails> Create(RoomCancellationPolicy roomCancellationPolicy, DateTime checkInDate, DateTime checkOutDate, PaymentDetails paymentDetails)
        {
            var cancellationPolicyData = roomCancellationPolicy.Policies;
            var cancellationPolicyDetails = new List<CancellationPolicyDetails>(cancellationPolicyData.Count);

            foreach (var cancellationPolicyDataItem in cancellationPolicyData)
            {
                var startDate = checkInDate.Date.AddDays(-cancellationPolicyDataItem.DaysPriorToArrival.ToDay);
                var endDate = checkInDate.Date.AddDays(-cancellationPolicyDataItem.DaysPriorToArrival.FromDay);

                decimal pricePenalty;
                double percent;
                if (cancellationPolicyDataItem.PenaltyType == PolicyPenaltyTypes.Percent)
                {
                    pricePenalty = CalculatePercentPenaltyPrice(cancellationPolicyDataItem.PenaltyCharge, paymentDetails);
                    percent = cancellationPolicyDataItem.PenaltyCharge;
                }
                else
                {
                    pricePenalty = CalculateNightsPenaltyPrice((int) cancellationPolicyDataItem.PenaltyCharge, paymentDetails);
                    percent = Math.Truncate(Convert.ToDouble((checkOutDate - checkInDate).Days / 100) * cancellationPolicyDataItem.PenaltyCharge);
                }
                  
                var cancellationDetails = new CancellationPolicyDetails(startDate, endDate, pricePenalty, percent);
                cancellationPolicyDetails.Add(cancellationDetails);
            }

            return cancellationPolicyDetails;
        }
        

        private decimal CalculatePercentPenaltyPrice(double percentToCharge, PaymentDetails paymentDetails)
        => MoneyRounder.Ceil(paymentDetails.PriceTotal * Convert.ToDecimal(percentToCharge) / 100, paymentDetails.Currency);


        private decimal CalculateNightsPenaltyPrice(int nightsToCharge, PaymentDetails paymentDetails)
        {
            var penaltyPrice = 0m;
            var nights = nightsToCharge;
            foreach (var seasonPrice in paymentDetails.SeasonPrices)
            {
                var nightsNumberLeft = seasonPrice.NumberOfNights - nights;
                if (nightsNumberLeft >= 0)
                {
                    penaltyPrice += seasonPrice.RatePrice * nights;
                    return penaltyPrice;
                }
                penaltyPrice += seasonPrice.RatePrice * seasonPrice.NumberOfNights;
                nights = Math.Abs(nightsNumberLeft);
            }

            return penaltyPrice;
        }
    }
}