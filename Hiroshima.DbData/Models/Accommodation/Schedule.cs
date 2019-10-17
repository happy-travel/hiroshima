using System;

namespace Hiroshima.DbData.Models.Accommodation
{
    public class Schedule
    {
        public TimeSpan CheckInTime { get; set; }
        public TimeSpan CheckOutTime { get; set; }
        public TimeSpan PortersStartTime { get; set; }
        public TimeSpan PortersEndTime { get; set; }
        public TimeSpan RoomServiceStartTime { get; set; }
        public TimeSpan RoomServiceEndTime { get; set; }
    }
}
