using System;
using HappyTravel.EdoContracts.Accommodations;
using HappyTravel.Hiroshima.Common.Models.Availabilities;
using HappyTravel.Hiroshima.Common.Models.Enums;

namespace HappyTravel.Hiroshima.Common.Models.Bookings
{
    public class Booking
    {
        public Guid Id { get; set; }
        public string ReferenceCode { get; set; }
        public BookingStatuses Status { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public AvailabilityRequest AvailabilityRequest { get; set; }
        public BookingRequest BookingRequest { get; set; }
        public AvailableRates Rates {get; set; }
        public string LanguageCode { get; set; }
        public int ContractManagerId { get; set; }
        public ContractManager ContractManager { get; set; }
    }
}
