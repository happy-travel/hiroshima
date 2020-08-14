using System.ComponentModel.DataAnnotations;

namespace HappyTravel.Hiroshima.DirectManager.Models.Requests
{
    public class Picture
    {
        [Required]
        public string Source { get; set; }
        public string Caption { get; set; }
    }
}