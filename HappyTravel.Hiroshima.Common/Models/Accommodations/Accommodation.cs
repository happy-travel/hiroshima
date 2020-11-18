using System;
using System.Collections.Generic;
using System.Text.Json;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.OccupancyDefinitions;
using HappyTravel.Hiroshima.Common.Models.Enums;
using HappyTravel.Hiroshima.Common.Models.Images;
using NetTopologySuite.Geometries;

namespace HappyTravel.Hiroshima.Common.Models.Accommodations
{
    public class Accommodation
    {
        public int Id { get; set; }

        public JsonDocument Name { get; set; }
        
        public JsonDocument Address { get; set; }
        
        public JsonDocument TextualDescription { get; set; }
        
        public JsonDocument LeisureAndSports { get; set; }
        
        public Point Coordinates { get; set; }
        
        public AccommodationStars Rating { get; set; }

        public string CheckInTime { get; set; } = string.Empty;

        public string CheckOutTime { get; set; } = string.Empty;
        
        public JsonDocument Pictures { get; set; }
        
        public ContactInfo ContactInfo { get; set; }
        
        public PropertyTypes PropertyType { get; set; }
        
        public int? BuildYear { get; set; }
        
        public int? Floors { get; set; }
        
        public JsonDocument AccommodationAmenities { get; set; }
        
        public JsonDocument AdditionalInfo { get; set; }
        
        public OccupancyDefinition OccupancyDefinition { get; set; }
        
        public int ContractManagerId { get; set; }
        
        public int LocationId { get; set; }
        
        public ContractManager ContractManager { get; set; }
        
        public Locations.Location Location { get; set; }
        
        public List<Room> Rooms { get; set; } = new List<Room>();

        public RateOptions RateOptions { get; set; } = new RateOptions{SingleAdultAndChildBookings = SingleAdultAndChildBookings.ApplyAdultRate};

        public Status Status { get; set; }
        
        public DateTime Created { get; set; }
        
        public DateTime Modified { get; set; }

        public List<Image> Images { get; set; } = new List<Image>();
        
        
        public override bool Equals(object? obj) => obj is Accommodation other && Equals(other);

        
        public override int GetHashCode() => HashCode.Combine(Id, Coordinates, Name.RootElement.ToString());


        public bool Equals(Accommodation other) => Id == other.Id && Coordinates.Equals(other.Coordinates) && Name.RootElement.ToString().Equals(other.Name.RootElement.ToString());
    }
}