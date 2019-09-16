using System;

namespace Hiroshima.DirectContracts.Models.Common
{
    public class DcPermittedOccupancy
    {
        public int AdultsCount { get; set; }
        public int ChildrenCount { get; set; }

        public override int GetHashCode()
        {
            return HashCode.Combine(AdultsCount, ChildrenCount);
        }
        private bool Equals(DcPermittedOccupancy other)
        {
            return (AdultsCount, ChildrenCount)
                .Equals((other.AdultsCount, other.ChildrenCount));
        }
        public override bool Equals(Object obj)
        {
            return obj is DcPermittedOccupancy other && Equals(other);
        }
    }
}
