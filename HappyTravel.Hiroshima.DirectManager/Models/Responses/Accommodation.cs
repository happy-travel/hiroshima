using System.Collections.Generic;
using HappyTravel.Geography;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Common.Models.Accommodations;
using HappyTravel.Hiroshima.Common.Models.Enums;
using HappyTravel.Hiroshima.Data.Models.Accommodations;
using HappyTravel.Hiroshima.DirectManager.Models.Requests;
using NetTopologySuite.Geometries;

namespace HappyTravel.Hiroshima.DirectManager.Models.Responses
{
    public struct Accommodation
    {
        public int Id { get; set; }
        public MultiLanguage<string> Name { get; set; }
        public MultiLanguage<string> Address { get; set; }
        public MultiLanguage<string> TextualDescription { get; set; }
        public GeoPoint Coordinates { get; set; }
        public AccommodationRating Rating { get; set; }
        public string CheckInTime { get; set; }
        public string CheckOutTime { get; set; }
        public MultiLanguage<List<Picture>> Pictures { get; set; }
        public ContactInfo ContactInfo { get; set; }
        public PropertyTypes PropertyType { get; set; }
        public MultiLanguage<List<string>> Amenities { get; set; }
        public MultiLanguage<string> AdditionalInfo { get; set; }
        public OccupancyDefinition OccupancyDefinition { get; set; }
        public List<int> RoomIds { get; set; }
    }
}