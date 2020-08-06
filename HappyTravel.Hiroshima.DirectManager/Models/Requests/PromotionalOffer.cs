using System;
using System.ComponentModel.DataAnnotations;
using HappyTravel.Hiroshima.Common.Models;

namespace HappyTravel.Hiroshima.DirectManager.Models.Requests
{
    public class PromotionalOffer
    {
        [Required]
        public int RoomId { get; set; }
        
        [Required]
        public DateTime BookByDate { get; set; }
        
        [Required]
        public DateTime ValidFrom { get; set; }
        
        [Required]
        public DateTime ValidTo { get; set; }
        
        [Required]
        public double DiscountPercent { get; set; }
        
        public string BookingCode { get; set; }
        
        [Required]
        public MultiLanguage<string> Details { get; set; }
    }
}