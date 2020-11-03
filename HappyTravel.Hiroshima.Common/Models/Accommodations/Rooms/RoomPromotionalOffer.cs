﻿using System;
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
        
        public decimal DiscountPercent { get; set; }
        
        public string? BookingCode { get; set; }
        
        public int ContractId { get; set; }
        
        public JsonDocument Description { get; set; }
        
        public Room Room { get; set; }
        
        public Contract Contract { get; set; }
    }
}