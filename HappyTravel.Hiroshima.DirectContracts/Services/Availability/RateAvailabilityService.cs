using System;
using System.Collections.Generic;
using System.Linq;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms;
using HappyTravel.Hiroshima.DirectContracts.Models;

namespace HappyTravel.Hiroshima.DirectContracts.Services.Availability
{
    public class RateAvailabilityService : IRateAvailabilityService
    {
        public RateAvailabilityService(IPaymentDetailsService paymentDetailsService, ICancellationPolicyService cancellationPolicyService)
        {
            _cancellationPolicyService = cancellationPolicyService;
            _paymentDetailsService = paymentDetailsService;
        }


        public List<RateDetails> GetAvailableRates(List<Room> rooms, DateTime checkInDate, DateTime checkOutDate)
            => rooms.Select(room =>
                {
                    var rates = room.RoomRates;
                    var promotionalOffers = room.RoomPromotionalOffers;
                    var paymentDetails = _paymentDetailsService.Create(checkInDate, checkOutDate, rates, promotionalOffers);
                    var cancellationPolicy = room.RoomCancellationPolicies.First(policy
                        => policy.Season.SeasonRanges.Any(seasonRange => seasonRange.StartDate <= checkInDate && checkInDate <= seasonRange.EndDate));
                    var cancellationPolicyDetails = _cancellationPolicyService.Create(cancellationPolicy, checkInDate, paymentDetails);
                    var firstRate = rates.First();
                    var mealPlan = firstRate.MealPlan;
                    var boardBasis = firstRate.BoardBasis;

                    return new RateDetails(room, paymentDetails, cancellationPolicyDetails, mealPlan, boardBasis, new List<TaxDetails>(), new List<string>());
                })
                .ToList();
        
        
        private readonly IPaymentDetailsService _paymentDetailsService;
        private readonly ICancellationPolicyService _cancellationPolicyService;
    }
}