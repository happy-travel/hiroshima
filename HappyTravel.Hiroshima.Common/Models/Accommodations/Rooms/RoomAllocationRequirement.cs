using HappyTravel.Hiroshima.Common.Models.Seasons;

namespace HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms
{
    public class RoomAllocationRequirement
    {
        public int Id { get; set; }
        
        public int RoomId { get; set; }
        
        public int SeasonRangeId { get; set; }
        
        public int ReleaseDays { get; set; }
        
        public int? MinimumLengthOfStay { get; set; }
        
        public int? Allotment { get; set; }
        
        public Room Room { get; set; }
        
        public SeasonRange SeasonRange { get; set; }
    }
}