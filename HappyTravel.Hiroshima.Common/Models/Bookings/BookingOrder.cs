using System;
using System.Text.Json;
using HappyTravel.Hiroshima.Common.Models.Enums;

namespace HappyTravel.Hiroshima.Common.Models.Bookings
{
    public class BookingOrder
    {
        public Guid Id { get; set; }
        
        public string ReferenceCode { get; set; } = string.Empty;
        
        public BookingStatuses Status { get; set; }
        
        public DateTime Created { get; set; }
        
        public DateTime Modified { get; set; }
        
        public DateTime CheckInDate { get; set; }
        
        public DateTime CheckOutDate { get; set; }
        
        /// <summary>
        /// EdoContracts.Accommodation.Availability.AvailabilityRequest
        /// </summary>
        public JsonDocument AvailabilityRequest { get; set; }
        
        /// <summary>
        /// EdoContracts.Accommodation.BookingRequest
        /// </summary>
        public JsonDocument BookingRequest { get; set; }
        
        /// <summary>
        /// Models.Availabilities.AvailableRates
        /// </summary>
        public JsonDocument AvailableRates {get; set; }
        
        public string LanguageCode { get; set; } = string.Empty;
        
        public int ManagerId { get; set; }
        
        public Manager Manager { get; set; }
    }
}
