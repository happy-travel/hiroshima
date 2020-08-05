using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Common.Models.Accommodations;
using Picture = HappyTravel.Hiroshima.Common.Models.Picture;

 namespace HappyTravel.Hiroshima.DirectManager.Models.Requests
{
    public class RoomRequest
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