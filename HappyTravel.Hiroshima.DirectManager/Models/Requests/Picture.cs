using System.ComponentModel.DataAnnotations;
using HappyTravel.Hiroshima.Common.Models.Enums;

namespace HappyTravel.Hiroshima.DirectManager.Models.Requests
{
    public class Picture
    {
        [Required]
        public string Source { get; set; }
        
        [Required]
        public PictureTypes Type { get; set; }
        
        public string Caption { get; set; }
    }
}