using System;

namespace HappyTravel.Hiroshima.DirectManager.Models.Requests
{
    public class PromotionalOfferStopSale
    {
        public int RoomId { get; set; }
        
        public DateTime FromDate { get; set; }
        
        public DateTime ToDate { get; set; }
    }
}