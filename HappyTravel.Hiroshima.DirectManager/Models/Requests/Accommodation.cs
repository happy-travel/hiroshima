using System.Collections.Generic;
using HappyTravel.Geography;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Common.Models.Accommodations;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.OccupancyDefinitions;
using HappyTravel.Hiroshima.Common.Models.Enums;
using PropertyTypes = HappyTravel.Hiroshima.Common.Models.Accommodations.PropertyTypes;

namespace HappyTravel.Hiroshima.DirectManager.Models.Requests
{
    public class Accommodation
    {
        public MultiLanguage<string> Name { get; set; }
        
        public MultiLanguage<string> Address { get; set; }
        
        public MultiLanguage<TextualDescription> Description { get; set; }
        
        public GeoPoint Coordinates { get; set; }
        
        public AccommodationStars Rating { get; set; }
        
        public string CheckInTime { get; set; }
        
        public string CheckOutTime { get; set; }
       
        public ContactInfo ContactInfo { get; set; }
        
        public PropertyTypes Type { get; set; }
        
        public MultiLanguage<List<string>> Amenities { get; set; } = new MultiLanguage<List<string>>{ En = new List<string>() };

        public MultiLanguage<string> AdditionalInfo { get; set; } = new MultiLanguage<string> { En = string.Empty };
        
        public OccupancyDefinition OccupancyDefinition { get; set; }
        
        public MultiLanguage<List<string>> LeisureAndSports { get; set; } = new MultiLanguage<List<string>> { En = new List<string>() };
        
        public int? BuildYear { get; set; }
        
        public int? Floors { get; set; }
        
        public RateOptions RateOptions { get; set; } = new RateOptions {SingleAdultAndChildBookings = SingleAdultAndChildBookings.ApplyAdultRate};
        
        public Status Status { get; set; }
        
        public int LocationId { get; set; }
    }
}