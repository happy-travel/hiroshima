using System;
using System.Collections.Generic;
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

        public MultiLanguage<string> Name { get; set; }
        
        public MultiLanguage<string> Address { get; set; }
        
        public MultiLanguage<TextualDescription> TextualDescription { get; set; }
        
        public MultiLanguage<List<string>> LeisureAndSports { get; set; }
        
        public Point Coordinates { get; set; }
        
        public AccommodationStars Rating { get; set; }

        public string CheckInTime { get; set; } = string.Empty;

        public string CheckOutTime { get; set; } = string.Empty;
        
        public ContactInfo ContactInfo { get; set; } = new ContactInfo();
        
        public PropertyTypes PropertyType { get; set; }
        
        public int? BuildYear { get; set; }
        
        public int? Floors { get; set; }
        
        public MultiLanguage<List<string>> AccommodationAmenities { get; set; }
        
        public MultiLanguage<string> AdditionalInfo { get; set; }
        
        public OccupancyDefinition OccupancyDefinition { get; set; }
        
        public int ContractManagerId { get; set; }
        
        public int LocationId { get; set; }
        
        public ContractManager ContractManager { get; set; }
        
        public Locations.Location Location { get; set; }
        
        public RateOptions RateOptions { get; set; } = new RateOptions{SingleAdultAndChildBookings = SingleAdultAndChildBookings.ApplyAdultRate};

        public Status Status { get; set; }
        
        public DateTime Created { get; set; }
        
        public DateTime Modified { get; set; }
        
        [Newtonsoft.Json.JsonIgnore]
        public List<SlimImage> Images { get; set; } = new List<SlimImage>();
        
        [Newtonsoft.Json.JsonIgnore]
        public List<Room> Rooms { get; set; } = new List<Room>();
        
        
        public override bool Equals(object? obj) => obj is Accommodation other && Equals(other);


        public bool Equals(Accommodation other) => (Id, Coordinates, Name, Address, ContactInfo, OccupancyDefinition, LocationId).Equals((other.Id, other.Coordinates, other.Name, other.Address, other.ContactInfo, other.OccupancyDefinition, other.LocationId))
            && Rooms.SafeSequenceEqual(other.Rooms);
        
        public override int GetHashCode() => Hash.Aggregate<Room>(Rooms, HashCode.Combine(Id, Coordinates, Name));
    }
}