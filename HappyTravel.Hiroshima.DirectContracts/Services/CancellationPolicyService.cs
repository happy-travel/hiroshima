using System;
using System.Collections.Generic;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.CancellationPolicies;
using HappyTravel.Hiroshima.DirectContracts.Models;
using HappyTravel.Money.Helpers;
using HappyTravel.Money.Models;

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

                MoneyAmount pricePenalty;
                decimal percent;
                if (cancellationPolicyDataItem.PenaltyType == PolicyPenaltyTypes.Percent)
                {
                    pricePenalty = CalculatePercentPenaltyPrice(cancellationPolicyDataItem.PenaltyCharge, paymentDetails);
                    percent = cancellationPolicyDataItem.PenaltyCharge;
                }
                else
                {
                    pricePenalty = CalculateNightsPenaltyPrice((int) cancellationPolicyDataItem.PenaltyCharge, paymentDetails);
                    percent = Math.Truncate(Convert.ToDecimal((checkOutDate - checkInDate).Days / 100) * cancellationPolicyDataItem.PenaltyCharge);
                }
                  
                var cancellationDetails = new CancellationPolicyDetails(startDate, endDate, pricePenalty, percent);
                cancellationPolicyDetails.Add(cancellationDetails);
            }

            return cancellationPolicyDetails;
        }
        

        private MoneyAmount CalculatePercentPenaltyPrice(decimal percentToCharge, PaymentDetails paymentDetails)
            => new MoneyAmount(MoneyRounder.Ceil(paymentDetails.TotalAmount.Amount * percentToCharge / 100, paymentDetails.TotalAmount.Currency), paymentDetails.TotalAmount.Currency);


        private MoneyAmount CalculateNightsPenaltyPrice(int nightsToCharge, PaymentDetails paymentDetails)
        {
            var penaltyPrice = 0m;
            var nights = nightsToCharge;
            foreach (var seasonPrice in paymentDetails.SeasonPrices)
            {
                var nightsNumberLeft = seasonPrice.NumberOfNights - nights;
                if (nightsNumberLeft >= 0)
                {
                    penaltyPrice += seasonPrice.RateAmount.Amount * nights;
                    return new MoneyAmount(penaltyPrice, paymentDetails.TotalAmount.Currency);
                }
                penaltyPrice += seasonPrice.RateAmount.Amount * seasonPrice.NumberOfNights;
                nights = Math.Abs(nightsNumberLeft);
            }

            return new MoneyAmount(penaltyPrice, paymentDetails.TotalAmount.Currency);
        }
    }
}