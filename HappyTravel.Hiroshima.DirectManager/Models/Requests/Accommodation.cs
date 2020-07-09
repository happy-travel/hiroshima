using System.Collections.Generic;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Common.Models.Accommodations;
using HappyTravel.Hiroshima.Common.Models.Enums;
using HappyTravel.Hiroshima.Data.Models.Accommodations;
using NetTopologySuite.Geometries;

namespace HappyTravel.Hiroshima.DirectManager.Models.Requests
{
    public class Accommodation
    {
        public MultiLanguage<string> Name { get; set; }
        public MultiLanguage<string> Address { get; set; }
        public MultiLanguage<string> TextualDescription { get; set; }
        public Coordinate Coordinates { get; set; }
        public AccommodationRating Rating { get; set; }
        public string CheckInTime { get; set; }
        public string CheckOutTime { get; set; }
        public MultiLanguage<List<Picture>> Pictures { get; set; }
        public Contacts Contacts { get; set; }
        public AccommodationTypes Type { get; set; }
        public MultiLanguage<List<string>> AccommodationAmenities { get; set; }
        public MultiLanguage<string> AdditionalInfo { get; set; }
        public OccupancyDefinition OccupancyDefinition { get; set; }
        public List<Room> Rooms { get; set; }
    }
}