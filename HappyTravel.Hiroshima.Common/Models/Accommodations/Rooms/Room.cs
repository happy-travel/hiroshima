using System;
using System.Collections.Generic;
using System.Text.Json;
using HappyTravel.EdoContracts.Extensions;
using HappyTravel.Hiroshima.Common.Infrastructure.Utilities;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.CancellationPolicies;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.OccupancyDefinitions;

namespace HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms
{
    [Serializable]
    public class Room
    {
        public int Id { get; set; }
        
        public int AccommodationId { get; set; }
        
        public JsonDocument Name { get; set; }
        
        public JsonDocument Description { get; set; }
        
        public JsonDocument Amenities { get; set; }
        
        public JsonDocument Pictures { get; set; }
        
        public DateTime Created { get; set; }
        
        public DateTime Modified { get; set; }
        
        public List<OccupancyConfiguration> OccupancyConfigurations { get; set; } = new List<OccupancyConfiguration>();
        
        public Accommodation Accommodation { get; set; }
        
        public List<RoomRate> RoomRates { get; set; }
        
        public List<RoomPromotionalOffer> RoomPromotionalOffers { get; set; } = new List<RoomPromotionalOffer>();

        public List<PromotionalOfferStopSale> PromotionalOffersStopSale { get; set; } = new List<PromotionalOfferStopSale>();

        public List<RoomAllocationRequirement> RoomAllocationRequirements { get; set; } = new List<RoomAllocationRequirement>();
        
        public List<RoomAvailabilityRestriction> RoomAvailabilityRestrictions { get; set; } = new List<RoomAvailabilityRestriction>();
        
        public List<RoomCancellationPolicy> RoomCancellationPolicies { get; set; } = new List<RoomCancellationPolicy>();
        
        
        public override bool Equals(object? obj) => obj is Room other && Equals(other);


        public override int GetHashCode()
            => Hash.Aggregate<OccupancyConfiguration>(OccupancyConfigurations, HashCode.Combine(Id, AccommodationId, Name.RootElement.ToString(), Description.RootElement.ToString(), Amenities.RootElement.ToString()));
            
        
        public bool Equals(Room other)
        {
            return Id == other.Id && AccommodationId == other.AccommodationId &&
                   Name.RootElement.ToString().Equals(other.Name.RootElement.ToString()) &&
                   Description.RootElement.ToString().Equals(other.Description.RootElement.ToString()) &&
                   Amenities.RootElement.ToString().Equals(other.Amenities.RootElement.ToString()) &&
                   OccupancyConfigurations.SafeSequenceEqual(other.OccupancyConfigurations);
        }
    }
}