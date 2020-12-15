using System.IO;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.EdoContracts.Accommodations;
using HappyTravel.Hiroshima.Common.Models.Enums;
using HappyTravel.Hiroshima.DirectManager.Models.Webhooks.Bookings;

namespace HappyTravel.Hiroshima.DirectManager.Services.Bookings
{
    public interface IBookingWebhookService
    {
        Task<Result> Send(string bookingReferenceCode, BookingStatuses bookingStatus);

        Task<Result<BookingWebhookData>> Get(Stream stream);
    }
}