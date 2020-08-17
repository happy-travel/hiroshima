using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Common.Models.Accommodations;

 namespace HappyTravel.Hiroshima.DirectManager.Models.Requests
{
    public class Room
    {
        [Required]
        public MultiLanguage<string> Name { get; set; }
        
        [Required]
        public MultiLanguage<string> Description{ get; set; }
        
        [Required]
        public MultiLanguage<List<string>> Amenities{ get; set; }
        
        [Required]
        public MultiLanguage<List<Picture>> Pictures { get; set; }
        
        [Required]
        public List<OccupancyConfiguration> OccupancyConfigurations { get; set; }
    }
}