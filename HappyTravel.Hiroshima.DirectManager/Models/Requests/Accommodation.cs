using System.Collections.Generic;
using HappyTravel.Geography;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Common.Models.Accommodations;
using HappyTravel.Hiroshima.Common.Models.Enums;
using NetTopologySuite.Geometries;
using ContactInfo = HappyTravel.Hiroshima.Common.Models.ContactInfo;
using Picture = HappyTravel.Hiroshima.Common.Models.Picture;

namespace HappyTravel.Hiroshima.DirectManager.Models.Requests
{
    public class Accommodation
    {
        public MultiLanguage<string> Name { get; set; }
        public MultiLanguage<string> Address { get; set; }
        public MultiLanguage<string> Description { get; set; }
        public GeoPoint Coordinates { get; set; }
        public AccommodationRating Rating { get; set; }
        public string CheckInTime { get; set; }
        public string CheckOutTime { get; set; }
        public MultiLanguage<List<Picture>> Pictures { get; set; }
        public ContactInfo ContactInfo { get; set; }
        public PropertyTypes Type { get; set; }
        public MultiLanguage<List<string>> Amenities { get; set; }
        public MultiLanguage<string> AdditionalInfo { get; set; }
        public OccupancyDefinition OccupancyDefinition { get; set; }
        public int LocationId { get; set; }
    }
}