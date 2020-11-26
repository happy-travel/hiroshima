using System;
using System.Collections.Generic;
using HappyTravel.EdoContracts.Extensions;
using HappyTravel.Hiroshima.Common.Infrastructure.Utilities;

namespace HappyTravel.Hiroshima.Common.Models
{
    public class ContactInfo
    {
        public List<string> Emails { get; set; } = new List<string>();

        public List<string> Phones { get; set; } = new List<string>();

        public List<string> Websites { get; set; } = new List<string>();

        
        public override bool Equals(object? obj) => obj is ContactInfo other && Equals(other);


        public bool Equals(in ContactInfo other)
            => Emails.SafeSequenceEqual(other.Emails) && Phones.SafeSequenceEqual(other.Phones) && Websites.SafeSequenceEqual(other.Websites);

        
        public override int GetHashCode() =>  Hash.Aggregate(Emails, Hash.Aggregate(Phones, Hash.Aggregate(Websites, 0)));
    }
}
