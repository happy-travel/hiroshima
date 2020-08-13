using System.Collections.Generic;

namespace HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.CancellationPolicies
{
    public class RoomCancellationPolicy
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int SeasonId { get; set; }
        public List<Policy> Policies { get; set; }
    }
}