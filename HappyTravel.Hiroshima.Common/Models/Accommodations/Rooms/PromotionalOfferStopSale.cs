using System;

namespace HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms
{
    public class PromotionalOfferStopSale
    {
        public int Id { get; set; }
        
        public int RoomId { get; set; }
        
        public DateTime FromDate { get; set; }
        
        public DateTime ToDate { get; set; }
        
        public int ContractId { get; set; }

        public Room Room { get; set; }

        public Contract Contract { get; set; }

    }
}