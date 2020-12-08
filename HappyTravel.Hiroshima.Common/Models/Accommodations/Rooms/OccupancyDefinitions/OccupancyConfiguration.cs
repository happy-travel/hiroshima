using System;

namespace HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.OccupancyDefinitions
{
    public class OccupancyConfiguration
    {
        public int Adults { get; set; }
        
        public int Teenagers { get; set; }
        
        public int Children { get; set; }
        
        public int Infants { get; set; }

        
        public override bool Equals(object obj)
        {
            return obj is OccupancyConfiguration other && Equals(other);
        }

        
        public bool Equals(OccupancyConfiguration other) =>
            Adults == other.Adults && Teenagers == other.Teenagers && Children == other.Children &&
            Infants == other.Infants;
        
        
        public override int GetHashCode() => HashCode.Combine(Adults, Teenagers, Children, Infants);
    }
}