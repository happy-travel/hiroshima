namespace HappyTravel.Hiroshima.Data.Models.Rooms
{
    public class RoomAllocationRequirement
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int SeasonRangeId { get; set; }
        public int ReleaseDays { get; set; }
        public int? MinimumLengthOfStay { get; set; }
        public int? Allotment { get; set; }
    }
}