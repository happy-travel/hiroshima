using System.ComponentModel.DataAnnotations;

namespace HappyTravel.Hiroshima.DirectManager.Models.Requests
{
    public class Location
    {
        [Required]
        public string Country { get; set; } 
        
        [Required]
        public string Locality { get; set; }
        
        public string Zone { get; set; } 
    }
}