using System;

namespace Hiroshima.DbData.Models.Accommodation
{
    public class Schedule
    {
        public string CheckInTime { get; set; }
        public string CheckOutTime { get; set; }
        public string PortersStartTime { get; set; }
        public string PortersEndTime { get; set; }
        public string RoomServiceStartTime { get; set; }
        public string RoomServiceEndTime { get; set; }
    }
}
