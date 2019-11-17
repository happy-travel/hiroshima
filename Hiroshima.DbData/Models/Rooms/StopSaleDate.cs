using System;

namespace Hiroshima.DbData.Models.Rooms
{
    public class StopSaleDate
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}