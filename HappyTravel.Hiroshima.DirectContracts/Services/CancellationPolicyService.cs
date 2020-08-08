using System;
using System.Collections.Generic;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.CancellationPolicies;
using HappyTravel.Hiroshima.DirectContracts.Models;
using HappyTravel.Money.Helpers;

namespace HappyTravel.Hiroshima.DirectContracts.Services
{
    public class CancellationPolicyService: ICancellationPolicyService
    {
        public List<CancellationPolicyDetails> Get(RoomCancellationPolicy roomCancellationPolicy, DateTime checkInDate, PaymentDetails paymentDetails)
        {
            var cancellationPolicyData = roomCancellationPolicy.Policies;
            var cancellationPolicyDetails = new List<CancellationPolicyDetails>(cancellationPolicyData.Count);
            
            foreach (var cancellationPolicyDataItem in cancellationPolicyData)
            {
                var cancellationDetails = new CancellationPolicyDetails
                (
                    startDate: checkInDate.Date.AddDays(-cancellationPolicyDataItem.DayPriorToArrival.ToDay),
                    endDate: checkInDate.Date.AddDays(-cancellationPolicyDataItem.DayPriorToArrival.FromDay),
                    price: cancellationPolicyDataItem.PenaltyType == CancellationPenaltyTypes.Percent
                        ? CalculatePercentPenaltyPrice(cancellationPolicyDataItem.PenaltyCharge, paymentDetails)
                        : CalculateNightsPenaltyPrice((int) cancellationPolicyDataItem.PenaltyCharge, paymentDetails)
                );
                cancellationPolicyDetails.Add(cancellationDetails);
            }

            return cancellationPolicyDetails;
        }
        

        private decimal CalculatePercentPenaltyPrice(double percentToCharge, PaymentDetails paymentDetails)
        => MoneyRounder.Ceil(paymentDetails.TotalPrice * Convert.ToDecimal(percentToCharge) / 100, paymentDetails.Currency);


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