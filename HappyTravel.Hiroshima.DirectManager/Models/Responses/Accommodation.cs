using System.Collections.Generic;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Common.Models.Accommodations;
using HappyTravel.Hiroshima.Common.Models.Enums;
using HappyTravel.Hiroshima.Data.Models.Accommodations;
using HappyTravel.Hiroshima.DirectManager.Models.Requests;
using NetTopologySuite.Geometries;

namespace HappyTravel.Hiroshima.DirectManager.Models.Responses
{
    public class Accommodation
    {
        public int Id { get; set; }
        public MultiLanguage<string> Name { get; set; }
        public MultiLanguage<string> Address { get; set; }
        public MultiLanguage<string> TextualDescription { get; set; }
        public Coordinate Coordinates { get; set; }
        public AccommodationRating Rating { get; set; }
        public string CheckInTime { get; set; }
        public string CheckOutTime { get; set; }
        public MultiLanguage<List<Picture>> Pictures { get; set; }
        public ContactInfo ContactInfo { get; set; }
        public PropertyTypes PropertyType { get; set; }
        public MultiLanguage<string> AccommodationAmenities { get; set; }
        public MultiLanguage<string> AdditionalInfo { get; set; }
        public OccupancyDefinition OccupancyDefinition { get; set; }
        public List<Room> Rooms { get; set; }
    }
}