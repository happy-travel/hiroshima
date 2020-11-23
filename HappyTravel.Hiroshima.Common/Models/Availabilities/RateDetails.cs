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
            BoardBasisTypes boardBasis, List<TaxDetails> taxes, List<string> amenities, string description)
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
            Amenities = amenities ?? new List<string>();
        }


        public override bool Equals(object? obj) => obj is RateDetails other && Equals(other);


        public bool Equals(in RateDetails other)
            => (Room, RoomType, OccupationRequest, PaymentDetails, BoardBasis, MealPlan, Description).Equals((other.Room, other.RoomType, other.OccupationRequest, other.PaymentDetails, other.BoardBasis, other.MealPlan, other.Description))
                && CancellationPolicies.SafeSequenceEqual(other.CancellationPolicies)
                && Taxes.SafeSequenceEqual(other.Taxes)
                && Amenities.SafeSequenceEqual(other.Amenities);

        
        public override int GetHashCode() => Hash.Aggregate<CancellationPolicyDetails>(CancellationPolicies, Hash.Aggregate<TaxDetails>(Taxes, Hash.Aggregate<string>(Amenities, HashCode.Combine(Room, RoomType, OccupationRequest, PaymentDetails, BoardBasis, MealPlan, Description))));
        
        
        public RateDetailsSlim RateDetailsSlim => new RateDetailsSlim(OccupationRequest, Room.Id, RoomType, PaymentDetails, CancellationPolicies, MealPlan, BoardBasis, Taxes, Amenities, Description);
        
        
        public Room Room { get; }
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