using System;

namespace Hiroshima.DbData.Models.Rooms
{
    public class RoomAllocationRequirement
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public DateTime StartsFromDate { get; set; }
        public DateTime EndsToDate { get; set; }
        public ReleasePeriod ReleasePeriod { get; set; }
        public int? MinimumStayNights { get; set; }
        public int? Allotment { get; set; }
    }
}