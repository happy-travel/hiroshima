using System;
using System.Collections.Generic;
using Hiroshima.Common.Models;
using Hiroshima.Common.Models.Enums;
using Hiroshima.DbData.Models.Rooms;

namespace Hiroshima.DbData.Models.Booking
{
    public class Booking
    {
        public int Id { get; set; }
        public string ReferenceCode { get; set; }
        public string LanguageCode { get; set; }
        public BookingStatusCodes StatusCode { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public string Nationality { get; set; }
        public string Residency { get; set; }
        public List<RoomGuests> Rooms { get; set; }
    }
}
