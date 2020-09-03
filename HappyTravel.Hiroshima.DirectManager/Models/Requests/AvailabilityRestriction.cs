using System;
using System.ComponentModel.DataAnnotations;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms;

namespace HappyTravel.Hiroshima.DirectManager.Models.Requests
{
    public class AvailabilityRestriction
    {
        [Required]
        public int RoomId { get; set; }
        
        [Required]
        public DateTime FromDate { get; set; }
        
        [Required]
        public DateTime ToDate { get; set; }
        
        [Required]
        public AvailabilityRestrictions Restriction { get; set; }
    }
}