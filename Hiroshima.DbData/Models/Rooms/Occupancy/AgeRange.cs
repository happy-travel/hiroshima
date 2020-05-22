namespace Hiroshima.DbData.Models.Rooms.Occupancy
{
    public class AgeRange
    {
        public int LowerBound { get; set; }
        public int UpperBound { get; set; }
        public bool LowerBoundInclusive { get; set; }
        public bool UpperBoundInclusive { get; set; }
    }
}