using System.Collections.Generic;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.CancellationPolicies;

namespace HappyTravel.Hiroshima.DirectManager.Models.Responses
{
    public readonly struct CancellationPolicy
    {
        public CancellationPolicy(int id, int roomId, int seasonId, List<CancellationPolicyItem> policies)
        {
            Id = id;
            RoomId = roomId;
            SeasonId = seasonId;
            Policies = policies;
        }
        
        public int Id { get; }
        public int RoomId { get; }
        public int SeasonId { get; }
        public List<CancellationPolicyItem> Policies { get; }
    }
}