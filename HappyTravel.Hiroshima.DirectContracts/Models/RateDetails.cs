using System.Collections.Generic;
using HappyTravel.EdoContracts.Accommodations.Enums;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms;

namespace HappyTravel.Hiroshima.DirectContracts.Models
{
    public readonly struct RateDetails
    {
        public RateDetails(Room room, PaymentDetails paymentDetails, List<CancellationPolicyDetails> cancellationPolicies, string meal,
            BoardBasisTypes boardBasis, List<TaxDetails> taxes, List<string> amenities)
        {
            Room = room;
            PaymentDetails = paymentDetails;
            Meal = meal;
            BoardBasis = boardBasis;
            CancellationPolicies = cancellationPolicies ?? new List<CancellationPolicyDetails>();
            Taxes = taxes ?? new List<TaxDetails>();
            Amenities = amenities ?? new List<string>();
        }

        
        public Room Room { get; }
        public PaymentDetails PaymentDetails { get; }
        public List<CancellationPolicyDetails> CancellationPolicies { get; }
        public List<TaxDetails> Taxes { get; }
        public List<string> Amenities { get; }
        public string Meal { get; }
        public BoardBasisTypes BoardBasis { get; }
    }
}