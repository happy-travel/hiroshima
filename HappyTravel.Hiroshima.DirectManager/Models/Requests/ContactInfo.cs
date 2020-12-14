
using System.Collections.Generic;

namespace HappyTravel.Hiroshima.DirectManager.Models.Requests
{
    public class ContactInfo
    {
        public List<string> Emails { get; set; }
        
        public List<string> Phones { get; set; } 
        
        public List<string> Websites { get; set; }
    }
}