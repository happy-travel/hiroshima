using System.Collections.Generic;
using HappyTravel.Hiroshima.Common.Models.Seasons;

namespace HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.CancellationPolicies
{
    public class RoomCancellationPolicy
    {
        public int Id { get; set; }
        
        public int RoomId { get; set; }
        
        public int SeasonId { get; set; }

        public List<Policy> Policies { get; set; } = new List<Policy>();
        
        public Room Room { get; set; }
        
        public Season Season { get; set; }
    }
}