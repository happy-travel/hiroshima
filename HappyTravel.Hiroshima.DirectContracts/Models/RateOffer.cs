﻿using System.Collections.Generic;
using HappyTravel.Hiroshima.DbData.Models.Room;

namespace HappyTravel.Hiroshima.DirectContracts.Models
{
    public struct RateOffer
    {
        public Room Room { get; set; }
        public PaymentDetails PaymentDetails { get; set; }
        public List<CancellationPolicyDetails> CancellationPolicies { get; set; }
        public List<TaxDetails> Taxes { get; set; }
        public List<string> Amenities { get; set; } 
        public string Meal { get; set; }
        public string BoardBasis { get; set; }
    }
}