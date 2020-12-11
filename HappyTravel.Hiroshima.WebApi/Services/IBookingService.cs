using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace HappyTravel.Hiroshima.WebApi.Services
{
    public interface IBookingService
    {
        Task<Result<EdoContracts.Accommodations.Booking>> Book(EdoContracts.Accommodations.BookingRequest bookingRequest, string languageCode);

        Task<Result<EdoContracts.Accommodations.Booking>> GetDetails(string bookingReferenceCode);

        Task<Result> Cancel(string bookingReferenceCode);
    }
}