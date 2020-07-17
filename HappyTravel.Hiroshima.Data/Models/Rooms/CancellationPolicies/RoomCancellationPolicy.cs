using System;
using System.Collections.Generic;

namespace HappyTravel.Hiroshima.Data.Models.Rooms.CancellationPolicies
{
    public class RoomCancellationPolicy
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int SeasonId { get; set; }
        public List<CancellationPolicyData> Details { get; set; }
    }
}