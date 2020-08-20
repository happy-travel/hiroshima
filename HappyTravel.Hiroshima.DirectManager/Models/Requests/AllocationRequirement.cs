using System.ComponentModel.DataAnnotations;

namespace HappyTravel.Hiroshima.DirectManager.Models.Requests
{
    public class AllocationRequirement
    {
        [Required]
        public int SeasonRangeId { get; set; }
        
        [Required]
        public int RoomId { get; set; }
        
        [Required]
        public int ReleaseDays { get; set; }
        
        [Required]
        public int Allotment { get; set; }
        
        public int? MinimumLengthOfStay { get; set; }
    }
}