using System;
using HappyTravel.Hiroshima.Common.Models.Enums;

namespace HappyTravel.Hiroshima.WebApi.Services.AvailabilitySearch
{
    public class BookingResponseService : IBookingResponseService
    {
        public EdoContracts.Accommodations.Booking Create(Common.Models.Bookings.Booking booking)
        {
            var referenceCode = booking.ReferenceCode;
            var status = GetStatus(booking.Status);
            
            var accommodationId = booking.Rates.AccommodationId;
            var supplierReferenceCode = booking.Rates.Id.ToString();
            var checkInDate = booking.AvailabilityRequest.CheckInDate;
            var checkOutDate = booking.AvailabilityRequest.CheckOutDate;
            var rooms = booking.BookingRequest.Rooms;
            var updateMode = EdoContracts.Accommodations.Enums.BookingUpdateModes.Asynchronous; 
            
            return new EdoContracts.Accommodations.Booking(referenceCode, status, accommodationId.ToString(), supplierReferenceCode, checkInDate, checkOutDate, rooms, updateMode);
        }


        private EdoContracts.Accommodations.Enums.BookingStatusCodes GetStatus(BookingStatuses status) => status switch
        {
            BookingStatuses.Processing => EdoContracts.Accommodations.Enums.BookingStatusCodes.WaitingForResponse,
            BookingStatuses.Confirmed => EdoContracts.Accommodations.Enums.BookingStatusCodes.Confirmed,
            BookingStatuses.Cancelled => EdoContracts.Accommodations.Enums.BookingStatusCodes.Cancelled,
            BookingStatuses.Rejected => EdoContracts.Accommodations.Enums.BookingStatusCodes.Cancelled,
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, "Failed to retrieve booking status")
        };
    }
}