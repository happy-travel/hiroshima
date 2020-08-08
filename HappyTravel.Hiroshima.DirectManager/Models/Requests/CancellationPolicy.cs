using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.CancellationPolicies;

namespace HappyTravel.Hiroshima.DirectManager.Models.Requests
{
    public class CancellationPolicy
    {
        [Required]
        public int RoomId { get; set; }
        
        [Required]
        public int SeasonId { get; set; }
        
        [Required]
        public List<CancellationPolicyItem> Policies { get; set; }
    }
}