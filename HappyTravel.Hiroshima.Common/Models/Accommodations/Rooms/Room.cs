using System;
using System.Collections.Generic;
using HappyTravel.EdoContracts.Extensions;
using HappyTravel.Hiroshima.Common.Infrastructure.Utilities;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.CancellationPolicies;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.OccupancyDefinitions;
using HappyTravel.Hiroshima.Common.Models.Images;

namespace HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms
{
    public class Room
    {
        public int Id { get; set; }
        
        public int AccommodationId { get; set; }
        
        public MultiLanguage<string> Name { get; set; }
        
        public MultiLanguage<string> Description { get; set; }
        
        public MultiLanguage<List<string>> Amenities { get; set; }
        
        public DateTime Created { get; set; }
        
        public DateTime Modified { get; set; }
        
        public List<OccupancyConfiguration> OccupancyConfigurations { get; set; } = new List<OccupancyConfiguration>();
        
        public Accommodation Accommodation { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public List<RoomRate> RoomRates { get; set; } = new List<RoomRate>();
        
        [Newtonsoft.Json.JsonIgnore]
        public List<RoomPromotionalOffer> RoomPromotionalOffers { get; set; } = new List<RoomPromotionalOffer>();
        
        [Newtonsoft.Json.JsonIgnore]
        public List<PromotionalOfferStopSale> PromotionalOffersStopSale { get; set; } = new List<PromotionalOfferStopSale>();

        [Newtonsoft.Json.JsonIgnore]
        public List<RoomAllocationRequirement> AllocationRequirements { get; set; } = new List<RoomAllocationRequirement>();
        
        [Newtonsoft.Json.JsonIgnore]
        public List<RoomAvailabilityRestriction> AvailabilityRestrictions { get; set; } = new List<RoomAvailabilityRestriction>();
       
        [Newtonsoft.Json.JsonIgnore]
        public List<RoomOccupancy> RoomOccupations { get; set; } = new List<RoomOccupancy>();

        public List<RoomCancellationPolicy> CancellationPolicies { get; set; } = new List<RoomCancellationPolicy>();

        public List<SlimImage> Images { get; set; } = new List<SlimImage>();
        
        //todo remove
        public MultiLanguage<List<Picture>> Pictures { get; set; }
        
        
        public override bool Equals(object? obj) => obj is Room other && Equals(other);


        public bool Equals(Room other)
        {
            return Id == other.Id && AccommodationId == other.AccommodationId &&
                Name.Equals(other.Name) &&
                Description.Equals(other.Description) &&
                OccupancyConfigurations.SafeSequenceEqual(other.OccupancyConfigurations);

        }


        public override int GetHashCode()
            => Hash.Aggregate<OccupancyConfiguration>(OccupancyConfigurations, HashCode.Combine(Id, AccommodationId, Name, Description));
    }
}