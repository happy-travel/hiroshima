using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;

namespace HappyTravel.Hiroshima.WebApi.Services
{
    public interface IBookingService
    {
        Task<Result<EdoContracts.Accommodations.Booking>> Book(EdoContracts.Accommodations.BookingRequest bookingRequest, string languageCode);
        
        Task<Result<EdoContracts.Accommodations.Booking>> GetDetails(string bookingReferenceCode, string languageCode);

        Task<Result> Cancel(string bookingReferenceCode, string languageCode);
    }
}