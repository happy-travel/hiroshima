using System;

namespace Hiroshima.DbData.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public BookingStatus Status { get; set; }
        public int StatusId { get; set; }
        public Rate Rate { get; set; }
        public int RateId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime BookingAt { get; set; }
        public DateTime CheckInAt { get; set; }
        public DateTime CheckOutAt { get; set; }
        public string Nationality { get; set; }
        public string Residency { get; set; }
        public MultiLanguage<string> MainPassengerName { get; set; }
    }
}
