using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HappyTravel.Geography;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Common.Models.Accommodations;
using HappyTravel.Hiroshima.Common.Models.Enums;
using Picture = HappyTravel.Hiroshima.Common.Models.Picture;

namespace HappyTravel.Hiroshima.DirectManager.Models.Requests
{
    public class Accommodation
    {
        [Required]
        public MultiLanguage<string> Name { get; set; }
        
        [Required]
        public MultiLanguage<string> Address { get; set; }
        
        [Required]
        public MultiLanguage<TextualDescription> Description { get; set; }
        
        public GeoPoint Coordinates { get; set; }
        
        [Required]
        public AccommodationRating Rating { get; set; }
        
        [Required]
        public string CheckInTime { get; set; }
        
        [Required]
        public string CheckOutTime { get; set; }
        
        [Required]
        public MultiLanguage<List<Picture>> Pictures { get; set; }
        
        public ContactInfo ContactInfo { get; set; }
        
        [Required]
        public PropertyTypes Type { get; set; }
        
        [Required]
        public MultiLanguage<List<string>> Amenities { get; set; }
        
        public MultiLanguage<string> AdditionalInfo { get; set; }
        
        [Required]
        public OccupancyDefinition OccupancyDefinition { get; set; }
        
        [Required]
        public int LocationId { get; set; }
    }
}