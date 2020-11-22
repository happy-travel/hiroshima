using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace HappyTravel.Hiroshima.DirectContracts.Services
{
    public interface IBookingService
    {
        Task<Result<Common.Models.Bookings.Booking>> Book (EdoContracts.Accommodations.BookingRequest rooms, EdoContracts.Accommodations.AvailabilityRequest availabilityRequest, string languageCode);
    }
}