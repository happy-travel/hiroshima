using System;
using System.Collections.Generic;
using HappyTravel.Hiroshima.Common.Models.Enums;

namespace HappyTravel.Hiroshima.Data.Models.Booking
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
