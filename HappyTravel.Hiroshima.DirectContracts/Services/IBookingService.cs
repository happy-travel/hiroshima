using System;
using System.Threading.Tasks;
using HappyTravel.Hiroshima.DirectContracts.Models;

namespace HappyTravel.Hiroshima.DirectContracts.Services
{
    public interface IBookingService
    {
        Task<BookingDetails> Book(Guid availableRatesId, EdoContracts.Accommodations.BookingRequest rooms, EdoContracts.Accommodations.AvailabilityRequest availabilityRequest);
    }
}