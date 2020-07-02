using System;

namespace Hiroshima.DbData.Models.Room.Occupancy
{
    public class OccupancyConfiguration
    {
        public int Adults { get; set; }
        public int Teenagers { get; set; }
        public int Children { get; set; }
        public int Infants { get; set; }

        
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;

            return Equals((OccupancyConfiguration) obj);
        }

        
        public override int GetHashCode() => HashCode.Combine(Adults, Teenagers, Children, Infants);

        
        public bool Equals(OccupancyConfiguration other) =>
            Adults == other.Adults && Teenagers == other.Teenagers && Children == other.Children &&
            Infants == other.Infants;
    }
}