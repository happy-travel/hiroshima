using System.Collections.Generic;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.CancellationPolicies;

namespace HappyTravel.Hiroshima.DirectManager.Models.Requests
{
    public class CancellationPolicy
    {
        public int RoomId { get; set; }
        
        public int SeasonId { get; set; }
        
        public List<Policy> Policies { get; set; }
    }
}