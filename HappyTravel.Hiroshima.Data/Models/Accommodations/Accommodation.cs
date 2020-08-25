using System.Collections.Generic;
using System.Text.Json;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Common.Models.Accommodations;
using HappyTravel.Hiroshima.Common.Models.Enums;
using HappyTravel.Hiroshima.Data.Models.Rooms;
using NetTopologySuite.Geometries;
using PropertyTypes = HappyTravel.Hiroshima.Common.Models.Accommodations.PropertyTypes;

namespace HappyTravel.Hiroshima.Data.Models.Accommodations
{
    public class Accommodation
    {
        public int Id { get; set; }
        public JsonDocument Name { get; set; }
        public JsonDocument Address { get; set; }
        public JsonDocument TextualDescription { get; set; }
        public Point Coordinates { get; set; }
        public AccommodationRating Rating { get; set; }
        public string CheckInTime { get; set; }
        public string CheckOutTime { get; set; }
        public JsonDocument Pictures { get; set; }
        public ContactInfo ContactInfo { get; set; }
        public PropertyTypes PropertyType { get; set; }
        public JsonDocument AccommodationAmenities { get; set; }
        public JsonDocument AdditionalInfo { get; set; }
        public OccupancyDefinition OccupancyDefinition { get; set; }
        public int LocationId { get; set; }
        public int ContractManagerId { get; set; }
        public List<Room> Rooms { get; set; }
    }
}