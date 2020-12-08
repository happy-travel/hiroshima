using System;
using HappyTravel.Hiroshima.Common.Infrastructure.Extensions;
using HappyTravel.Hiroshima.Common.Models.Availabilities;
using HappyTravel.Hiroshima.Common.Models.Enums;

namespace HappyTravel.Hiroshima.WebApi.Services.AvailabilitySearch
{
    public class BookingResponseService : IBookingResponseService
    {
        public EdoContracts.Accommodations.Booking Create(Common.Models.Bookings.BookingOrder bookingOrder)
        {
            var referenceCode = bookingOrder.ReferenceCode;
            var status = GetStatus(bookingOrder.Status);
            var availabilityRequest = bookingOrder.AvailabilityRequest.GetValue<EdoContracts.Accommodations.AvailabilityRequest>();
            var availableRates = bookingOrder.AvailableRates.GetValue<AvailableRates>();
            var bookingRequest = bookingOrder.BookingRequest.GetValue<EdoContracts.Accommodations.BookingRequest>();
            var accommodationId = availableRates.AccommodationId.ToString();
            var supplierReferenceCode = availableRates.Id.ToString();
            var checkInDate = availabilityRequest.CheckInDate;
            var checkOutDate = availabilityRequest.CheckOutDate;
            var rooms = bookingRequest.Rooms;
            var updateMode = EdoContracts.Accommodations.Enums.BookingUpdateModes.Asynchronous;
            
            return new EdoContracts.Accommodations.Booking(referenceCode, status, accommodationId, supplierReferenceCode, checkInDate, checkOutDate, rooms, updateMode);
        }


        private EdoContracts.Accommodations.Enums.BookingStatusCodes GetStatus(BookingStatuses status) => status switch
        {
            BookingStatuses.Processing => EdoContracts.Accommodations.Enums.BookingStatusCodes.WaitingForResponse,
            BookingStatuses.WaitingForConfirmation => EdoContracts.Accommodations.Enums.BookingStatusCodes.WaitingForResponse,
            BookingStatuses.WaitingForCancellation => EdoContracts.Accommodations.Enums.BookingStatusCodes.WaitingForResponse,
            BookingStatuses.Confirmed => EdoContracts.Accommodations.Enums.BookingStatusCodes.Confirmed,
            BookingStatuses.Cancelled => EdoContracts.Accommodations.Enums.BookingStatusCodes.Cancelled,
            BookingStatuses.Rejected => EdoContracts.Accommodations.Enums.BookingStatusCodes.Cancelled,
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, "Failed to retrieve a booking status")
        };
    }
}