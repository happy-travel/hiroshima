using System;
using System.Collections.Generic;
using System.Linq;
using HappyTravel.EdoContracts.Accommodations.Internals;
using HappyTravel.Hiroshima.Common.Infrastructure.Extensions;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms;
using HappyTravel.Hiroshima.Common.Models.Availabilities;

namespace HappyTravel.Hiroshima.DirectContracts.Services.Availability
{
    public class RateAvailabilityService : IRateAvailabilityService
    {
        public RateAvailabilityService(IPaymentDetailsService paymentDetailsService, ICancellationPolicyService cancellationPolicyService)
        {
            _cancellationPolicyService = cancellationPolicyService;
            _paymentDetailsService = paymentDetailsService;
        }


        public List<RateDetails> GetAvailableRates(RoomOccupationRequest occupationRequest, List<Room> rooms, DateTime checkInDate, DateTime checkOutDate, string languageCode)
            => rooms.Select(room =>
                {
                    var rates = room.RoomRates;
                    var roomType = rates.First().RoomType;
                    var promotionalOffers = room.RoomPromotionalOffers;
                    var paymentDetails = _paymentDetailsService.Create(checkInDate, checkOutDate, rates, promotionalOffers, languageCode);
                    var cancellationPolicy = room.CancellationPolicies.First(policy
                        => policy.Season.SeasonRanges.Any(seasonRange => seasonRange.StartDate <= checkInDate && checkInDate <= seasonRange.EndDate));
                    var cancellationPolicyDetails = _cancellationPolicyService.Create(cancellationPolicy, checkInDate, checkOutDate, paymentDetails);
                    var firstRate = rates.First();
                    var mealPlan = firstRate.MealPlan;
                    var boardBasis = firstRate.BoardBasis;
                    room.Description.GetValue<MultiLanguage<string>>().TryGetValueOrDefault(languageCode, out var description);
                    
                    return new RateDetails(occupationRequest, room, roomType, paymentDetails, cancellationPolicyDetails, mealPlan, boardBasis, new List<TaxDetails>(), new List<string>(), description);
                })
                .ToList();
        
        
        private readonly IPaymentDetailsService _paymentDetailsService;
        private readonly ICancellationPolicyService _cancellationPolicyService;
    }
}