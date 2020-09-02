using System.ComponentModel.DataAnnotations;

namespace HappyTravel.Hiroshima.DirectManager.Models.Requests
{
    public class ContactInfo
    {
        public string Email { get; set; }
        
        public string Phone { get; set; }
        
        public string Website { get; set; }
    }
}