namespace HappyTravel.Hiroshima.Common.Models
{
    /// <summary>
    /// Represents an age range of a person. Bounds are included
    /// </summary>
    public struct AgeRange
    {
        public int? LowerBound { get; set; }
        public int? UpperBound { get; set; }
    }
}