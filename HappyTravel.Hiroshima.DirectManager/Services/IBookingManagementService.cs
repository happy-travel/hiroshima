using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public interface IBookingManagementService
    {
        Task<Result<List<Models.Responses.Bookings.BookingOrder>>> GetBookingOrders(Models.Requests.BookingRequest bookingRequest, int skip, int top,
            string languageCode);

        Task<Result> ConfirmBooking(Guid bookingId);
        
        Task<Result> TryCancel(Guid bookingId);
        
        Task<Result> ConfirmCancellation(Guid bookingId);
        
    }
}