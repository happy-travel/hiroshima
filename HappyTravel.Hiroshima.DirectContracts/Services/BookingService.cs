using System;
using System.Threading.Tasks;
using HappyTravel.EdoContracts.Accommodations;
using HappyTravel.Hiroshima.DirectContracts.Models;
using HappyTravel.Hiroshima.DirectContracts.Services.Availability;

namespace HappyTravel.Hiroshima.DirectContracts.Services
{
    public class BookingService : IBookingService
    {

        public BookingService(IAvailableRatesStorage availableRatesStorage)
        {
            _availableRatesStorage = availableRatesStorage;
        }


        public async Task<BookingDetails> Book(Guid availableRatesId, BookingRequest rooms, AvailabilityRequest availabilityRequest)
        {
            var availableRates = await _availableRatesStorage.Get(availableRatesId);
            //bookingRequest.AvailabilityId // GetSearchDetails
            //"book_hash": "h-a2206f15-4b96-57d1-ab06-f872284f576d"
            //"partner_order_id": "testRef125" 
            //"Rooms": [{"Guests":[{"Fn"}]}]
            //_s GetRate
            
            throw new NotImplementedException();
        }


        private readonly IAvailableRatesStorage _availableRatesStorage;
    }
}