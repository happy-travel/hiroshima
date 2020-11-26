using System;
using System.Collections.Generic;
using System.Text.Json;
using HappyTravel.EdoContracts.Extensions;
using HappyTravel.Hiroshima.Common.Infrastructure.Utilities;
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

        public ContactInfo ContactInfo { get; set; } = new ContactInfo();
        
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

        public List<SlimImage> Images { get; set; } = new List<SlimImage>();
        
        
        public override bool Equals(object? obj) => obj is Accommodation other && Equals(other);


        public bool Equals(Accommodation other) => (Id, Coordinates, Name.RootElement.ToString(), Address.RootElement.ToString(), ContactInfo, OccupancyDefinition, LocationId).Equals((other.Id, other.Coordinates, other.Name.RootElement.ToString(), other.Address.RootElement.ToString(), other.ContactInfo, other.OccupancyDefinition, other.LocationId))
            && Rooms.SafeSequenceEqual(other.Rooms);
        
        public override int GetHashCode() => Hash.Aggregate<Room>(Rooms, HashCode.Combine(Id, Coordinates, Name.RootElement.ToString()));
    }
}