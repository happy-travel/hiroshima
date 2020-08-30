using System;
using System.Collections.Generic;

namespace HappyTravel.Hiroshima.Data.Models
{
    public class Contract
    {
        public int Id { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ContractManagerId { get; set; }
        public List<Seasons.Season> Seasons { get; set; } 
        public List<Rooms.RoomPromotionalOffer> PromotionalOffers { get; set; } 
        public List<Rooms.RoomAvailabilityRestriction> RoomAvailabilityRestriction { get; set; } 
    }
}