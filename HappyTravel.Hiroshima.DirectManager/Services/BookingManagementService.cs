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
using HappyTravel.Hiroshima.Data;
using Microsoft.EntityFrameworkCore;
using PaymentDetails = HappyTravel.Hiroshima.DirectManager.Models.Responses.Bookings.PaymentDetails;
using RateDetails = HappyTravel.Hiroshima.Common.Models.Availabilities.RateDetails;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public class BookingManagementService : IBookingManagementService
    {
        public BookingManagementService(IContractManagerContextService contractManagerContext, DirectContractsDbContext  dbContext)
        {
            _contractManagerContext = contractManagerContext;
            _dbContext = dbContext;
        }
        
        
        public Task<Result<List<Models.Responses.Bookings.BookingOrder>>> GetBookingOrders(Models.Requests.BookingRequest bookingRequest, int skip, int top, string languageCode)
        {
            return _contractManagerContext.GetContractManager().Ensure(DoAllAccommodationIdsBelongToManager, "Invalid accommodation ids")
                .Map(_ => GetBookings())
                .Map(bookingOrders => Build(bookingOrders, languageCode));
            

            async Task<bool> DoAllAccommodationIdsBelongToManager(ContractManager contractManager)
            {
                if (!bookingRequest.AccommodationIds.Any())
                    return true;

                var existingIds = await _dbContext.Accommodations
                    .Where(a => bookingRequest.AccommodationIds.Contains(a.Id) && a.ContractManagerId == contractManager.Id)
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
        
        
        private PaymentDetails BuildPaymentDetails(AvailableRates availableRates)
        {
            var (amount, discount) = PriceHelper.GetPrice(availableRates.Rates);
            return new PaymentDetails(amount, discount);
        }


        private List<Models.Responses.Bookings.RateDetails> BuildRateDetails(List<RateDetails> availableRates, BookingRequest bookingRequest, string languageCode)
        {
            var rateDetails = new List<Models.Responses.Bookings.RateDetails>(availableRates.Count);
            for (var i = 0; i < availableRates.Count; i++)
            {
                var availableRate = availableRates[i];
                availableRate.Room.Name.TryGetValueOrDefault(languageCode, out var roomName);
                var roomOccupancy = BuildRoomOccupancy(bookingRequest.Rooms[i]);
                var paymentDetails = BuildPaymentDetails(availableRate);
                rateDetails.Add(new Models.Responses.Bookings.RateDetails(availableRate.Room.Id, roomName, roomOccupancy, paymentDetails, availableRate.CancellationPolicies));
            }

            return rateDetails;
        }


        private Models.Responses.Bookings.RoomOccupancy BuildRoomOccupancy(SlimRoomOccupation slimRoomOccupation)
            => new Models.Responses.Bookings.RoomOccupancy(slimRoomOccupation.Type, slimRoomOccupation.Passengers
                .Select(p=> new Models.Responses.Bookings.Pax(p.FirstName, p.LastName, p.IsLeader, p.Age))
                .ToList());
            
        
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
            
            return new Models.Responses.Bookings.BookingOrder(
                bookingOrder.Id.ToString(), 
                bookingOrder.Status, 
                bookingOrder.ReferenceCode,
                bookingOrder.CheckInDate, 
                bookingOrder.CheckOutDate, 
                rateDetails,
                paymentDetails,
                bookingOrder.AccommodationId, 
                accommodationName);
        }
        
        
        private readonly IContractManagerContextService _contractManagerContext;
        private readonly DirectContractsDbContext _dbContext;
    }
}