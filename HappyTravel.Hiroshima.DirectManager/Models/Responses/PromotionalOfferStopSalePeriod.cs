using System;

namespace HappyTravel.Hiroshima.DirectManager.Models.Responses
{
    public readonly struct PromotionalOfferStopSalePeriod
    {
        public PromotionalOfferStopSalePeriod(int id, int roomId, DateTime fromDate, DateTime toDate, int contractId)
        {
            Id = id;
            RoomId = roomId;
            FromDate = fromDate;
            ToDate = toDate;
            ContractId = contractId;
        }
        
        
        public int Id { get; }
        
        public int RoomId { get; }
        
        public DateTime FromDate { get; }
        
        public DateTime ToDate { get; }
        
        public int ContractId { get; }
    }
}