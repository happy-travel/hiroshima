using System;
using Hiroshima.Common.Models;
using Hiroshima.DbData.Models.Rates;
using Hiroshima.Common.Models.Enums;

namespace Hiroshima.DbData.Models.Booking
{
    public class Booking
    {
        public int Id { get; set; }
        public BookingStatusCodes StatusCode { get; set; }
        public ContractRate ContractRate { get; set; }
        public int ContractRateId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime BookedAt { get; set; }
        public DateTime CheckInAt { get; set; }
        public DateTime CheckOutAt { get; set; }
        public string Nationality { get; set; }
        public string Residency { get; set; }
        public string LeadPassengerName { get; set; }
    }
}
