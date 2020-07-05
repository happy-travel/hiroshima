using System;
using System.Collections.Generic;

namespace HappyTravel.Hiroshima.Data.Models.Rooms.CancellationPolicies
{
    public class RoomCancellationPolicy
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<CancellationPolicyData> Details { get; set; }
    }
}