using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Common.Models.Enums;

namespace HappyTravel.Hiroshima.DirectManager.Services.Bookings
{
    public interface IBookingWebhookService
    {
        Task<Result> Send(string bookingReferenceCode, BookingStatuses bookingStatus);

        bool IsSignatureValid(string signature, long timestamp);
    }
}