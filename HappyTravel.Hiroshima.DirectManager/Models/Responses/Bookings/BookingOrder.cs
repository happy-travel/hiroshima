using System;
using System.Collections.Generic;
using HappyTravel.Hiroshima.Common.Models.Enums;

namespace HappyTravel.Hiroshima.DirectManager.Models.Responses.Bookings
{
    public readonly struct BookingOrder
    {
        public BookingOrder(string id, BookingStatuses status, string referenceCode, DateTime checkInDate, DateTime checkOutDate, List<RateDetails> rateDetails, Models.Responses.Bookings.PaymentDetails paymentDetails, int accommodationId, string accommodationName)
        {
            Id = id;
            Status = status;
            ReferenceCode = referenceCode;
            CheckInDate = checkInDate;
            CheckOutDate = checkOutDate;
            RateDetails = rateDetails;
            PaymentDetails = paymentDetails;
            AccommodationId = accommodationId;
            AccommodationName = accommodationName;
        }


        public string Id { get; }
        
        public BookingStatuses Status { get; }
        
        public string ReferenceCode { get; }
        
        public DateTime CheckInDate { get; }
        
        public DateTime CheckOutDate { get; }
        
        public List<Models.Responses.Bookings.RateDetails> RateDetails { get; }
        
        public Models.Responses.Bookings.PaymentDetails PaymentDetails { get; }
        
        public int AccommodationId { get; }
        
        public string AccommodationName { get; }
    }
}