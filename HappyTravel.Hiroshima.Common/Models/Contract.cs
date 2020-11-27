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

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
        
        public DateTime Created { get; set; }
        
        public DateTime Modified { get; set; }
        
        public bool Verified { get; set; }
        
        public int ContractManagerId { get; set; }
        
        public Manager ContractManager { get; set; }

        public List<Season> Seasons { get; set; } = new List<Season>();

        public List<RoomPromotionalOffer> PromotionalOffers { get; set; } = new List<RoomPromotionalOffer>();

        public List<PromotionalOfferStopSale> PromotionalOffersStopSale { get; set; } = new List<PromotionalOfferStopSale>();

        public List<RoomAvailabilityRestriction> RoomAvailabilityRestrictions { get; set; } = new List<RoomAvailabilityRestriction>();

        public List<Document> Documents { get; set; } = new List<Document>();
    }
}