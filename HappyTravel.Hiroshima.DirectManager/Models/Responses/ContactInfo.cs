using System;
using System.Collections.Generic;
using System.Linq;

namespace HappyTravel.Hiroshima.DirectManager.Models.Responses
{
    public readonly struct ContactInfo
    {
        public ContactInfo(List<string> emails, List<string> phones, List<string> websites)
        {
            Emails = emails;
            Phones = phones;
            Websites = websites;
        }
        
        
        public List<string> Emails { get; }
        public List<string> Phones { get; } 
        public List<string> Websites { get; }


        public override bool Equals(object? obj)
            => obj is ContactInfo other && Emails.SequenceEqual(other.Emails) && Phones.SequenceEqual(other.Phones) && Websites.SequenceEqual(other.Websites);


        public override int GetHashCode() => HashCode.Combine(Emails, Phones, Websites);
    }
}