using System;
using System.Collections.Generic;
using HappyTravel.EdoContracts.Accommodations.Enums;
using HappyTravel.EdoContracts.Accommodations.Internals;
using HappyTravel.EdoContracts.Extensions;
using HappyTravel.Hiroshima.Common.Infrastructure.Utilities;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms;

namespace HappyTravel.Hiroshima.Common.Models.Availabilities
{
    public readonly struct RateDetails
    {
        public RateDetails(RoomOccupationRequest occupationRequest, Room room, RoomTypes roomType, PaymentDetails paymentDetails, List<CancellationPolicyDetails> cancellationPolicies, string mealPlan,
            BoardBasisTypes boardBasis, List<TaxDetails> taxes, string description)
        {
            Room = room;
            RoomType = roomType;
            PaymentDetails = paymentDetails;
            MealPlan = mealPlan;
            BoardBasis = boardBasis;
            CancellationPolicies = cancellationPolicies;
            Taxes = taxes ?? new List<TaxDetails>();
            Description = description;
            OccupationRequest = occupationRequest;
        }


        public override bool Equals(object? obj) => obj is RateDetails other && Equals(other);


        public bool Equals(in RateDetails other)
            => (Room, RoomType, OccupationRequest, PaymentDetails, BoardBasis, MealPlan, Description).Equals((other.Room, other.RoomType, other.OccupationRequest, other.PaymentDetails, other.BoardBasis, other.MealPlan, other.Description))
                && CancellationPolicies.SafeSequenceEqual(other.CancellationPolicies)
                && Taxes.SafeSequenceEqual(other.Taxes);

        
        public override int GetHashCode() => Hash.Aggregate<CancellationPolicyDetails>(CancellationPolicies, Hash.Aggregate<TaxDetails>(Taxes, HashCode.Combine(Room, RoomType, OccupationRequest, PaymentDetails, BoardBasis, MealPlan, Description)));
        
        public Room Room { get; }
        
        public RoomTypes RoomType { get; }
        
        public PaymentDetails PaymentDetails { get; }
        
        public List<CancellationPolicyDetails> CancellationPolicies { get; }
        
        public List<TaxDetails> Taxes { get; }
        
        public string MealPlan { get; }
        
        public BoardBasisTypes BoardBasis { get; }
        
        public string Description { get; }
        
        public RoomOccupationRequest OccupationRequest { get; }
    }
}