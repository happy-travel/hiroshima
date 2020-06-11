using System;

namespace Hiroshima.DbData.Models.Rooms
{
    public class RoomAvailabilityRestrictions
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public SaleRestrictions Restrictions { get; set; }
    }
}