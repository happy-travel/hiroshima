using System;
using System.Collections.Generic;
using System.Text.Json;
using HappyTravel.EdoContracts.Extensions;
using Hiroshima.Common.Infrastructure.Utilities;
using Hiroshima.DbData.Models.Room.Occupancy;

namespace Hiroshima.DbData.Models.Room
{
    public class Room
    {
        public int Id { get; set; }
        public int AccommodationId { get; set; }
        public JsonDocument Name { get; set; }
        public JsonDocument Description { get; set; }
        public JsonDocument Amenities { get; set; }
        public List<OccupancyConfiguration> OccupancyConfigurations { get; set; }

        
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;

            return Equals((Room) obj);
        }

        
        public override int GetHashCode()
        {
            var hash = 17;
            foreach (var occupancyConfiguration in OccupancyConfigurations) 
                hash = Hash.GetAggregate(occupancyConfiguration, hash);

            hash = Hash.GetAggregate(
                HashCode.Combine(Id, AccommodationId, Name.RootElement.ToString(), Description.RootElement.ToString(),
                    Amenities.RootElement.ToString()), hash);
            
            return hash;
        }

        
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