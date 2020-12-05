using System;

namespace HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms
{
    public class RoomAvailabilityRestriction
    {
        public int Id { get; set; }
        
        public int RoomId { get; set; }
        
        public DateTime FromDate { get; set; }
        
        public DateTime ToDate { get; set; }
        
        public AvailabilityRestrictions Restriction { get; set; }
        
        public int ContractId { get; set; }
        
        [Newtonsoft.Json.JsonIgnore]
        public Contract Contract { get; set; }
        
        [Newtonsoft.Json.JsonIgnore]
        public Room Room { get; set; }
        
    }
}