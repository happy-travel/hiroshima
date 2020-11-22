using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.WebApi.Services.AvailabilitySearch;


namespace HappyTravel.Hiroshima.WebApi.Services
{
    public class BookingService : IBookingService
    {
        public BookingService(DirectContracts.Services.IBookingService bookingService, IAvailabilitySearchStorage availabilitySearchStorage, IBookingResponseService bookingResponseService)
        {
            _bookingService = bookingService;
            _availabilitySearchStorage = availabilitySearchStorage;
            _bookingResponseService = bookingResponseService;
        }
        
        
        public async Task<Result<EdoContracts.Accommodations.Booking>> Book(EdoContracts.Accommodations.BookingRequest bookingRequest, string languageCode)
        {
            return await GetAvailabilityRequest()
                .Bind(ProcessBooking)
                .Map(BuildResponse);
            

            async Task<Result<EdoContracts.Accommodations.AvailabilityRequest>> GetAvailabilityRequest()
            {
                var availabilityRequest = await _availabilitySearchStorage.GetAvailabilityRequest(bookingRequest.AvailabilityId);

                return availabilityRequest.Equals(default)
                    ? Result.Failure<EdoContracts.Accommodations.AvailabilityRequest>("Failed to retrieve availability data")
                    : Result.Success(availabilityRequest);
            }


            async Task<Result<Common.Models.Bookings.Booking>> ProcessBooking(EdoContracts.Accommodations.AvailabilityRequest availabilityRequest)
            {
               return await _bookingService.Book(bookingRequest, availabilityRequest, languageCode);
            }


            EdoContracts.Accommodations.Booking BuildResponse(Common.Models.Bookings.Booking booking)
            {
                return _bookingResponseService.Create(booking);
            }
        }


        public Task<Result<EdoContracts.Accommodations.Booking>> GetDetails(string bookingReferenceCode, string languageCode) => throw new System.NotImplementedException();


        public Task<Result> Cancel(string bookingReferenceCode, string languageCode) => throw new System.NotImplementedException();

        private readonly DirectContracts.Services.IBookingService _bookingService;
        private readonly IAvailabilitySearchStorage _availabilitySearchStorage;
        private readonly IBookingResponseService _bookingResponseService;
    }
}