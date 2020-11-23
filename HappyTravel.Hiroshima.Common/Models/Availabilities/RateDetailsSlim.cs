using System;
using System.Collections.Generic;
using HappyTravel.EdoContracts.Accommodations.Enums;
using HappyTravel.EdoContracts.Accommodations.Internals;
using HappyTravel.EdoContracts.Extensions;
using HappyTravel.Hiroshima.Common.Infrastructure.Utilities;

namespace HappyTravel.Hiroshima.Common.Models.Availabilities
{
    public readonly struct RateDetailsSlim
    {
        public RateDetailsSlim(RoomOccupationRequest occupationRequest, int roomId, RoomTypes roomType, PaymentDetails paymentDetails, List<CancellationPolicyDetails> cancellationPolicies, string mealPlan,
            BoardBasisTypes boardBasis, List<TaxDetails> taxes, List<string> amenities, string description)
        {
            RoomId = roomId;
            RoomType = roomType;
            PaymentDetails = paymentDetails;
            PaymentDetails = new PaymentDetails();
            MealPlan = mealPlan;
            BoardBasis = boardBasis;
            CancellationPolicies = cancellationPolicies;
            Taxes = taxes ?? new List<TaxDetails>();
            Description = description;
            OccupationRequest = occupationRequest;
            Amenities = amenities ?? new List<string>();
        }

        
        public override bool Equals(object? obj) => obj is RateDetails other && Equals(other);


        public bool Equals(in RateDetailsSlim other)
            => (RoomId, RoomType, OccupationRequest, PaymentDetails, BoardBasis, MealPlan, Description).Equals((other.RoomId, other.RoomType, other.OccupationRequest, other.PaymentDetails, other.BoardBasis, other.MealPlan, other.Description))
                && CancellationPolicies.SafeSequenceEqual(other.CancellationPolicies)
                && Taxes.SafeSequenceEqual(other.Taxes)
                && Amenities.SafeSequenceEqual(other.Amenities);

        
        public override int GetHashCode() => Hash.Aggregate<CancellationPolicyDetails>(CancellationPolicies, Hash.Aggregate<TaxDetails>(Taxes, Hash.Aggregate<string>(Amenities, HashCode.Combine(RoomId, RoomType, OccupationRequest, PaymentDetails, BoardBasis, MealPlan, Description))));
        
        
        public int RoomId { get; }
        public RoomTypes RoomType { get; }
        public PaymentDetails PaymentDetails { get; }
        public List<CancellationPolicyDetails> CancellationPolicies { get; }
        public List<TaxDetails> Taxes { get; }
        public List<string> Amenities { get; }
        public string MealPlan { get; }
        public BoardBasisTypes BoardBasis { get; }
        public string Description { get; }
        public RoomOccupationRequest OccupationRequest { get; }
    }
}