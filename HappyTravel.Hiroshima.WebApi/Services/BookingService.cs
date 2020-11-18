using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.WebApi.Services.AvailabilitySearch;
using Microsoft.AspNetCore.Mvc;

namespace HappyTravel.Hiroshima.WebApi.Services
{
    public class BookingService : IBookingService
    {
        public BookingService(DirectContracts.Services.IBookingService bookingService, IAvailabilitySearchStorage availabilitySearchStorage)
        {
            _bookingService = bookingService;
            _availabilitySearchStorage = availabilitySearchStorage;
        }
        
        public async Task<Result<EdoContracts.Accommodations.Booking, ProblemDetails>> Book(EdoContracts.Accommodations.BookingRequest bookingRequest, string languageCode)
        {
            var availabilityRequest = await _availabilitySearchStorage.GetAvailabilityRequest(bookingRequest.AvailabilityId);
            var booking = await _bookingService.Book(bookingRequest.RoomContractSetId, bookingRequest, availabilityRequest);
            throw new NotImplementedException();
        }


        public Task<Result<EdoContracts.Accommodations.Booking, ProblemDetails>> GetDetails(string bookingReferenceCode, string languageCode) => throw new System.NotImplementedException();


        public Task<Result> Cancel(string bookingReferenceCode, string languageCode) => throw new System.NotImplementedException();

        private readonly DirectContracts.Services.IBookingService _bookingService;
        private readonly IAvailabilitySearchStorage _availabilitySearchStorage;
    }
}