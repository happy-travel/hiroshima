using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms;
using HappyTravel.Hiroshima.Common.Models.Seasons;

namespace HappyTravel.Hiroshima.Common.Models
{
    public class Contract
    {
        public int Id { get; set; }
        
        public DateTime ValidFrom { get; set; }
        
        public DateTime ValidTo { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public DateTime Created { get; set; }
        
        public DateTime Modified { get; set; }
        
        public bool Verified { get; set; }
        
        public int ContractManagerId { get; set; }
        
        public ContractManager ContractManager { get; set; }
        
        public List<Season> Seasons { get; set; }
        
        public List<RoomPromotionalOffer> PromotionalOffers { get; set; }
        
        public List<RoomAvailabilityRestriction> RoomAvailabilityRestriction { get; set; }

        //[NotMapped]
        public List<Document> Documents { get; set; }
    }
}