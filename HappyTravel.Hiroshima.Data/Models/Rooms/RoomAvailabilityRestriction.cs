using System;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms;

namespace HappyTravel.Hiroshima.Data.Models.Rooms
{
    public class RoomAvailabilityRestriction
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public AvailabilityRestrictions Restriction { get; set; }
        public int ContractId { get; set; }
    }
}