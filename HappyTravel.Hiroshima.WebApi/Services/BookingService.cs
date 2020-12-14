using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.EdoContracts.Accommodations.Internals;
using HappyTravel.Hiroshima.DirectManager.Services;
using HappyTravel.Hiroshima.WebApi.Services.AvailabilitySearch;

namespace HappyTravel.Hiroshima.WebApi.Services
{
    public class BookingService : IBookingService
    {
        public BookingService(DirectContracts.Services.IBookingService bookingService, IAvailabilitySearchStorage availabilitySearchStorage, IBookingResponseService bookingResponseService, IBookingManagementService bookingManagementService)
        {
            _bookingService = bookingService;
            _availabilitySearchStorage = availabilitySearchStorage;
            _bookingResponseService = bookingResponseService;
            _bookingManagementService = bookingManagementService;
        }
        
        
        public async Task<Result<EdoContracts.Accommodations.Booking>> Book(EdoContracts.Accommodations.BookingRequest bookingRequest, string languageCode)
        {
            return await GetAvailabilityRequest()
                .Bind(ProcessBooking)
                .Map(Build);
            

            async Task<Result<EdoContracts.Accommodations.AvailabilityRequest>> GetAvailabilityRequest()
            {
                var availabilityRequest = await _availabilitySearchStorage.GetAvailabilityRequest(bookingRequest.AvailabilityId);

                if (availabilityRequest.Equals(default))
                    return Result.Failure<EdoContracts.Accommodations.AvailabilityRequest>("Failed to retrieve availability data");
                
                await _availabilitySearchStorage.RemoveAvailabilityRequest(bookingRequest.AvailabilityId);
                
                return Result.Success(availabilityRequest);
            }


            Task<Result<Common.Models.Bookings.BookingOrder>> ProcessBooking(EdoContracts.Accommodations.AvailabilityRequest availabilityRequest)
            {
               return ValidateBookingRequest()
                    .Bind(() => _bookingService.Book(bookingRequest, availabilityRequest, languageCode)); 
               

               Task<Result> ValidateBookingRequest()
               {
                   return ValidateRooms()
                       .Bind(() => ValidateReferenceCode(bookingRequest.ReferenceCode))
                       .Bind(CheckIfBookingOrderNotExists);
                   
                   
                   Result ValidateRooms()
                   {
                       if (bookingRequest.Rooms.Count != availabilityRequest.Rooms.Count)
                           return Result.Failure("Invalid number of rooms");

                       for (var i = 0; i < bookingRequest.Rooms.Count; i++)
                       {
                           var bookingRequestRoom = bookingRequest.Rooms[i];
                           var availabilityRequestRoom = availabilityRequest.Rooms[i];
                           if (!RoomsAreEqual(bookingRequestRoom, availabilityRequestRoom))
                               return Result.Failure("Invalid rooms parameters");
                       }
                       return Result.Success();
                   }
                   
                   
                   bool RoomsAreEqual(SlimRoomOccupation bookingRequestRoom, RoomOccupationRequest slimOccupationRequest) => bookingRequestRoom.Type == slimOccupationRequest.Type && bookingRequestRoom.Passengers.Count ==
                       slimOccupationRequest.AdultsNumber + slimOccupationRequest.ChildrenAges?.Count;
               }
              

               async Task<Result> CheckIfBookingOrderNotExists()
               {
                   var (_, isFailure, _, _) = await _bookingService.Get(bookingRequest.ReferenceCode);
                   return isFailure
                       ? Result.Success()
                       : Result.Failure<Common.Models.Bookings.BookingOrder>($"The booking order with the reference code '{bookingRequest.ReferenceCode}' already exists");
               }
            }
            
            
            EdoContracts.Accommodations.Booking Build(Common.Models.Bookings.BookingOrder booking)
            {
                return _bookingResponseService.Create(booking);
            }
        }
        
        
        public async Task<Result<EdoContracts.Accommodations.Booking>> GetDetails(string bookingReferenceCode)
        {
            var bookingOrder = await _bookingService.Get(bookingReferenceCode);
            
            return bookingOrder.IsFailure 
                ? _bookingResponseService.CreateEmpty(bookingReferenceCode) 
                : _bookingResponseService.Create(bookingOrder.Value);
        }


        public Task<Result> Cancel(string bookingReferenceCode) 
            => ValidateReferenceCode(bookingReferenceCode)
            .Bind(() => _bookingService.Get(bookingReferenceCode))
            .Bind(bookingOrder => _bookingManagementService.TryCancel(bookingOrder.Id));


        Result ValidateReferenceCode(string bookingReferenceCode) => IsReferenceCodeValid(bookingReferenceCode)
            ? Result.Success()
            : Result.Failure($"Invalid booking reference code '{bookingReferenceCode}'");

        
        private bool IsReferenceCodeValid(string bookingReferenceCode)
            => !string.IsNullOrWhiteSpace(bookingReferenceCode) && bookingReferenceCode.Length <= BookingReferenceCodeMaxLength;
        
        
        private readonly DirectContracts.Services.IBookingService _bookingService;
        private readonly IAvailabilitySearchStorage _availabilitySearchStorage;
        private readonly IBookingResponseService _bookingResponseService;
        private readonly IBookingManagementService _bookingManagementService;

        private const int BookingReferenceCodeMaxLength = 36;
    }
}