using System;

namespace HappyTravel.Hiroshima.Common.Models
{
    public class ContactInfo
    {
        public string Email { get; set; }
        
        public string Phone { get; set; }
        
        public string Website { get; set; }
        
        
        public override bool Equals(object? obj) => obj is ContactInfo other && Equals(other);


        public bool Equals(in ContactInfo other) => (Email, Phone, Website).Equals((other.Email, other.Phone, other.Website));

        
        public override int GetHashCode() =>  HashCode.Combine(Email, Phone, Website);
    }
}
