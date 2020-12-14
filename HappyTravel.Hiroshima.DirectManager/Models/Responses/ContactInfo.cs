using System.Collections.Generic;

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
    }
}