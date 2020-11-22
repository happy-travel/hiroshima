using System;
using System.Threading.Tasks;
using HappyTravel.EdoContracts.Accommodations;
using HappyTravel.Hiroshima.DirectContracts.Models;
using HappyTravel.Hiroshima.DirectContracts.Services.Availability;

namespace HappyTravel.Hiroshima.DirectContracts.Services
{
    public class BookingService : IBookingService
    {

        public BookingService(IAvailabilityDataStorage availabilityDataStorage)
        {
            _availabilityDataStorage = availabilityDataStorage;
        }


        public async Task<BookingDetails> Book(Guid availableRatesId, BookingRequest bookingRequest, AvailabilityRequest availabilityRequest)
        {
            var rateDetailsHash = await _availabilityDataStorage.GetHash(bookingRequest.AvailabilityId, bookingRequest.RoomContractSetId);
           
            throw new NotImplementedException();
        }


        private readonly IAvailabilityDataStorage _availabilityDataStorage;
    }
}