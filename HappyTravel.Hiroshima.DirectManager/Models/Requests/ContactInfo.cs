using System.ComponentModel.DataAnnotations;

namespace HappyTravel.Hiroshima.DirectManager.Models.Requests
{
    public class ContactInfo
    {
        [Required]
        public string Email { get; set; }
        
        [Required]
        public string Phone { get; set; }
        
        public string Website { get; set; }
    }
}