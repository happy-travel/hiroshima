using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.EdoContracts.Accommodations;
using HappyTravel.EdoContracts.Accommodations.Internals;
using HappyTravel.Hiroshima.Common.Infrastructure.Extensions;
using HappyTravel.Hiroshima.Common.Infrastructure.Utilities;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Common.Models.Availabilities;
using HappyTravel.Hiroshima.Common.Models.Bookings;
using HappyTravel.Hiroshima.Common.Models.Enums;
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.DirectContracts.Extensions.FunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PaymentDetails = HappyTravel.Hiroshima.DirectManager.Models.Responses.Bookings.PaymentDetails;
using RateDetails = HappyTravel.Hiroshima.Common.Models.Availabilities.RateDetails;

namespace HappyTravel.Hiroshima.DirectManager.Services.Bookings
{
    public class BookingManagementService : IBookingManagementService
    {
        public BookingManagementService(IManagerContextService managerContextService, DirectContracts.Services.IBookingService bookingService, IBookingWebhookService bookingWebhookService, DirectContractsDbContext dbContext)
        {
            _managerContext = managerContextService;
            _bookingService = bookingService;
            _bookingWebhookService = bookingWebhookService;
            _dbContext = dbContext;
        }
        
        
        public Task<Result<List<Models.Responses.Bookings.BookingOrder>>> GetBookingOrders(Models.Requests.BookingRequest bookingRequest, int skip, int top, string languageCode)
        {
            return _managerContext.GetServiceSupplier().Ensure(AreAllAccommodationIdsBelongToSupplier, "Invalid accommodation ids")
                .Map(_ => GetBookings())
                .Map(bookingOrders => Build(bookingOrders, languageCode));
            

            async Task<bool> AreAllAccommodationIdsBelongToSupplier(ServiceSupplier supplier)
            {
                if (!bookingRequest.AccommodationIds.Any())
                    return true;

                var existingIds = await _dbContext.Accommodations
                    .Where(a => bookingRequest.AccommodationIds.Contains(a.Id) && a.ServiceSupplierId == supplier.Id)
                    .Select(bo => bo.Id)
                    .ToListAsync();

                return !bookingRequest.AccommodationIds.Except(existingIds).Any();
            }


            async Task<List<BookingOrder>> GetBookings()
            {
                var bookingOrders = _dbContext.BookingOrders.Where(bo => bookingRequest.BookingStatuses.Contains(bo.Status));
                if (!bookingRequest.FromDate.Equals(default))
                    bookingOrders = bookingOrders.Where(bo => bookingRequest.FromDate <= bo.Created);
            
                if (!bookingRequest.ToDate.Equals(default))
                    bookingOrders = bookingOrders.Where(bo => bo.Created <= bookingRequest.ToDate);

                if (bookingRequest.AccommodationIds.Any())
                    bookingOrders = bookingOrders.Where(bo
                        => bookingRequest.AccommodationIds.Contains(bo.AccommodationId));

                return await bookingOrders
                    .Skip(skip)
                    .Take(top)
                    .ToListAsync();
            }
        }


        public Task<Result> ConfirmBooking(Guid bookingId)
        {
            return _managerContext.GetServiceSupplier()
                .Check(supplier => CheckIfBookingBelongsToServiceSupplier(supplier, bookingId))
                .BindWithTransaction(_dbContext, manager 
                    => _bookingService.Confirm(bookingId)
                        .Bind(SendUpdateStatus));


            Task<Result> SendUpdateStatus(BookingOrder bookingOrder)
                => _bookingWebhookService.Send(bookingOrder.ReferenceCode, bookingOrder.Status);
        }


        public Task<Result> ConfirmCancellation(Guid bookingId)
        {
            return _managerContext.GetServiceSupplier()
                .Bind(supplier => CheckIfBookingBelongsToServiceSupplier(supplier, bookingId))
                .Ensure(IsValidStatus, "Failed to confirm the booking order" )
                .Bind(_ => _bookingService.Cancel(bookingId))
                .Bind(bookingOrder => _bookingWebhookService.Send(bookingOrder.ReferenceCode, BookingStatuses.Cancelled));


            bool IsValidStatus(BookingOrder bookingOrder) => bookingOrder.Status == BookingStatuses.WaitingForCancellation;
        }

        
        public Task<Result> TryCancel(Guid bookingId)
        {
            return _bookingService.Get(bookingId)
                .Bind(HandleCancellation);


            async Task<Result> HandleCancellation(BookingOrder bookingOrder)
            {
                switch (bookingOrder.Status)
                {
                    case BookingStatuses.Processing:
                    case BookingStatuses.WaitingForCancellation:
                    case BookingStatuses.Cancelled:
                        return await _bookingWebhookService.Send(bookingOrder.ReferenceCode, BookingStatuses.Cancelled);
                    case BookingStatuses.WaitingForConfirmation:
                        return await _bookingService.Cancel(bookingId)
                            .Bind(_ => _bookingWebhookService.Send(bookingOrder.ReferenceCode, BookingStatuses.Cancelled));
                    case BookingStatuses.Confirmed:
                        return await _bookingService.MarkAsWaitingForCancellation(bookingId)
                            .Bind(() => _bookingWebhookService.Send(bookingOrder.ReferenceCode, BookingStatuses.Processing));
                }
                
                return Result.Failure("Failed to cancel the booking order");
            }
        }
        
        
        private async Task<Result<BookingOrder>> CheckIfBookingBelongsToServiceSupplier(ServiceSupplier supplier, Guid bookingId)
        {
            var (_, isFailure, bookingOrder, error) = await _bookingService.Get(bookingId);
            if (isFailure)
                return Result.Failure<BookingOrder>(error);

            return bookingOrder.ServiceSupplierId == supplier.Id
                ? Result.Success(bookingOrder)
                : Result.Failure<BookingOrder>("The booking order does not belong to the service supplier");
        }

        
        private PaymentDetails BuildPaymentDetails(AvailableRates availableRates)
        {
            var (amount, discount) = PriceHelper.GetPrice(availableRates.Rates);
            return new PaymentDetails(amount, discount);
        }


        private List<Models.Responses.Bookings.RateDetails> BuildRateDetails(List<RateDetails> availableRates, BookingRequest bookingRequest,
            string languageCode)
        {
            var rateDetails = new List<Models.Responses.Bookings.RateDetails>(availableRates.Count);
            for (var i = 0; i < availableRates.Count; i++)
            {
                var availableRate = availableRates[i];
                availableRate.Room.Name.TryGetValueOrDefault(languageCode, out var roomName);
                var roomOccupancy = BuildRoomOccupancy(bookingRequest.Rooms[i]);
                var paymentDetails = BuildPaymentDetails(availableRate);
                rateDetails.Add(new Models.Responses.Bookings.RateDetails(availableRate.Room.Id, roomName, roomOccupancy, paymentDetails,
                    availableRate.CancellationPolicies));
            }

            return rateDetails;
        }


        private Models.Responses.Bookings.RoomOccupancy BuildRoomOccupancy(SlimRoomOccupation slimRoomOccupation)
            => new Models.Responses.Bookings.RoomOccupancy(slimRoomOccupation.Type,
                slimRoomOccupation.Passengers.Select(p => new Models.Responses.Bookings.Pax(p.FirstName, p.LastName, p.IsLeader, p.Age)).ToList());


        private PaymentDetails BuildPaymentDetails(RateDetails availableRate)
        {
            var totalAmount = availableRate.PaymentDetails.TotalAmount;
            var discount = availableRate.PaymentDetails.Discount;

            return new PaymentDetails(totalAmount, discount);
        }


        private List<Models.Responses.Bookings.BookingOrder> Build(List<BookingOrder> bookingOrders, string languageCode)
            => bookingOrders.Select(bo => Build(bo, languageCode)).ToList();


        private Models.Responses.Bookings.BookingOrder Build(BookingOrder bookingOrder, string languageCode)
        {
            var availableRates = bookingOrder.AvailableRates.GetValue<AvailableRates>();
            var bookingRequest = bookingOrder.BookingRequest.GetValue<BookingRequest>();
            var firstRoom = availableRates.Rates.First().Room;
            var accommodation = firstRoom.Accommodation;
            accommodation.Name.TryGetValueOrDefault(languageCode, out var accommodationName);
            var rateDetails = BuildRateDetails(availableRates.Rates, bookingRequest, languageCode);
            var paymentDetails = BuildPaymentDetails(availableRates);

            return new Models.Responses.Bookings.BookingOrder(bookingOrder.Id.ToString(), bookingOrder.Status, bookingOrder.ReferenceCode,
                bookingOrder.CheckInDate, bookingOrder.CheckOutDate, rateDetails, paymentDetails, bookingOrder.AccommodationId, accommodationName);
        }
        
        
        private readonly IManagerContextService _managerContext;
        private readonly DirectContracts.Services.IBookingService _bookingService;
        private readonly DirectContractsDbContext _dbContext;
        private readonly IBookingWebhookService _bookingWebhookService;
    }
}