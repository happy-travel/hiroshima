using System.ComponentModel.DataAnnotations;

namespace HappyTravel.Hiroshima.DirectManager.Models.Requests
{
    public class AllocationRequirement
    {
        public int SeasonRangeId { get; set; }
        
        public int RoomId { get; set; }
        
        public int ReleaseDays { get; set; }
        
        public int? Allotment { get; set; }
        
        public int? MinimumLengthOfStay { get; set; }
    }
}