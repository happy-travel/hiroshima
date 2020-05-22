﻿using System;
using Hiroshima.Common.Models;

namespace Hiroshima.DbData.Models.Rooms
{
    public class RoomPromotionalOffer
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public DateTime BookByDate { get; set; }
        public DateTime ValidFromDate { get; set; }
        public DateTime ValidToDate { get; set; }
        public double DiscountPercent { get; set; }
        public string BookingCode { get; set; }
        public MultiLanguage<string> Details { get; set; }
    }
}