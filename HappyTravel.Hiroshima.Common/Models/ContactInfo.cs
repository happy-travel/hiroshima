using System;
using System.Collections.Generic;
using HappyTravel.EdoContracts.Extensions;
using HappyTravel.Hiroshima.Common.Infrastructure.Utilities;

namespace HappyTravel.Hiroshima.Common.Models
{
    public class ContactInfo
    {
        public List<string> Email { get; set; } = new List<string>();

        public List<string> Phone { get; set; } = new List<string>();

        public List<string> Website { get; set; } = new List<string>();

        
        public override bool Equals(object? obj) => obj is ContactInfo other && Equals(other);


        public bool Equals(in ContactInfo other)
            => Email.SafeSequenceEqual(other.Email) && Phone.SafeSequenceEqual(other.Phone) && Website.SafeSequenceEqual(other.Website);

        
        public override int GetHashCode() =>  Hash.Aggregate(Email, Hash.Aggregate(Phone, Hash.Aggregate(Website, 0)));
    }
}
