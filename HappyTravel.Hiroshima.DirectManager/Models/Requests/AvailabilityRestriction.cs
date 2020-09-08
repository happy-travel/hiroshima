using System;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms;

namespace HappyTravel.Hiroshima.DirectManager.Models.Requests
{
    public class AvailabilityRestriction
    {
        public int RoomId { get; set; }
        
        public DateTime FromDate { get; set; }
        
        public DateTime ToDate { get; set; }
        
        public AvailabilityRestrictions Restriction { get; set; }
    }
}