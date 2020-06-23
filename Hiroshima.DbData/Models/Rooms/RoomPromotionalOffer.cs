﻿using System;
using System.Text.Json;
using Hiroshima.Common.Models;

namespace Hiroshima.DbData.Models.Rooms
{
    public class RoomPromotionalOffer:BaseModel
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public DateTime BookByDate { get; set; }
        public DateTime ValidFromDate { get; set; }
        public DateTime ValidToDate { get; set; }
        public double DiscountPercent { get; set; }
        public string BookingCode { get; set; }
        public JsonDocument Details { get; set; }
        
        
        public string GetDetailsFromFirstLanguage()
            => GetStringFromFirstLanguage(Details);
        
        
        public void SetDetails(MultiLanguage<string> details)
            => Details = CreateJDocument(details);       
    }
}