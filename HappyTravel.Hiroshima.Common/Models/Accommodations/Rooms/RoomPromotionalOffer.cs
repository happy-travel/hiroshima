using System;
using System.Text.Json;

namespace HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms
{
    public class RoomPromotionalOffer
    {
        public int Id { get; set; }
        
        public int RoomId { get; set; }
        
        public DateTime BookByDate { get; set; }
        
        public DateTime ValidFromDate { get; set; }
        
        public DateTime ValidToDate { get; set; }
        
        public double DiscountPercent { get; set; }
        
        public string? BookingCode { get; set; }
        
        public int ContractId { get; set; }
        
        public MultiLanguage<string> Description { get; set; }
        
        [Newtonsoft.Json.JsonIgnore]
        public Room Room { get; set; }
        
        [Newtonsoft.Json.JsonIgnore]
        public Contract Contract { get; set; }
    }
}