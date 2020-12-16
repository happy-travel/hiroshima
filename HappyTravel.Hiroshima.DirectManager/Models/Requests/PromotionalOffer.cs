using System;
using HappyTravel.Hiroshima.Common.Models;

namespace HappyTravel.Hiroshima.DirectManager.Models.Requests
{
    public class PromotionalOffer
    {
        public int RoomId { get; set; }
        
        public DateTime BookByDate { get; set; }
        
        public DateTime ValidFrom { get; set; }
        
        public DateTime ValidTo { get; set; }
        
        public double DiscountPercent { get; set; }
        
        public string BookingCode { get; set; }
        
        public MultiLanguage<string> Description { get; set; }
    }
}