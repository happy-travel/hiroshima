using HappyTravel.Hiroshima.Common.Infrastructure.Utilities;

namespace HappyTravel.Hiroshima.Common.Models
{
    /// <summary>
    /// Represents an age range of a person. Bounds are included
    /// </summary>
    public class AgeRange
    {
        public int LowerBound { get; set; }
        
        
        public int UpperBound { get; set; }
        
        
        public override bool Equals(object? obj) => obj is AgeRange other && Equals(other);
        
        
        public bool Equals(AgeRange other)
            => (LowerBound, UpperBound) == (other.LowerBound, other.UpperBound);
        
        
        public override int GetHashCode() => (LowerBound, UpperBound).GetHashCode();
    }
}