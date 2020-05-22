using System;

namespace Hiroshima.DbData.Models.Rooms
{
    public class RoomAvailabilityRestrictions
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public DateTime StartsFromDate { get; set; }
        public DateTime EndsToDate { get; set; }
        public SaleRestrictions Restrictions { get; set; }
    }
}
