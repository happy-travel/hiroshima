using System;
using HappyTravel.Hiroshima.Common.Models;

namespace HappyTravel.Hiroshima.DirectManager.Models.Responses
{
    public readonly struct PromotionalOffer
    {
        public PromotionalOffer(int id, int contractId, int roomId, DateTime bookBy, DateTime validFrom, DateTime validTo, double discountPercent, string bookingCode, MultiLanguage<string> remarks)
        {
            Id = id;
            ContractId = contractId;
            RoomId = roomId;
            ValidFrom = validFrom;
            ValidTo = validTo;
            BookBy = bookBy;
            DiscountPercent = discountPercent;
            BookingCode = bookingCode;
            Remarks = remarks;
        }


        public int Id { get; }
        public int ContractId { get; }
        public int RoomId { get; }
        public DateTime BookBy { get; }
        public DateTime ValidFrom { get; }
        public DateTime ValidTo { get; }
        public double DiscountPercent { get; }
        public string BookingCode { get; }
        public MultiLanguage<string> Remarks { get; }
    }
}