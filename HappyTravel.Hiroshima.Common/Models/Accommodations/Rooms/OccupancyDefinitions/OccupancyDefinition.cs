using System;

namespace HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.OccupancyDefinitions
{
    public class OccupancyDefinition
    {
        public AgeRange? Infant { get; set; }

        public AgeRange? Child { get; set; }

        public AgeRange? Teenager { get; set; }

        public AgeRange Adult { get; set; }


        public override bool Equals(object? obj)
        {
            if (!(obj is OccupancyDefinition other))
                return false;

            var isInfantEquals = (Infant == null && other.Infant == null) || (Infant != null && other.Infant != null && Infant.Equals(other.Infant));
            
            var isChildEquals = (Child == null && other.Child == null) || (Child != null && other.Child != null && Child.Equals(other.Child));
            
            var isTeenagerEquals = (Teenager == null && other.Teenager == null) || (Teenager != null && other.Teenager != null && Teenager.Equals(other.Teenager));
            
            return isInfantEquals && isChildEquals && isTeenagerEquals && Adult.Equals(other.Adult);
        }


        public override int GetHashCode() => HashCode.Combine(Infant, Child, Teenager, Adult);
    }
}