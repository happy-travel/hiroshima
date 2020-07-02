using System;

namespace HappyTravel.Hiroshima.DbData.Models.Room
{
    public class RoomAllocationRequirement
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ReleasePeriod ReleasePeriod { get; set; }
        public int? MinimumStayNights { get; set; }
        public int? Allotment { get; set; }
    }
}