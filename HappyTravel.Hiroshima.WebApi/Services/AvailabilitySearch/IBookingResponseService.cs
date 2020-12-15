namespace HappyTravel.Hiroshima.WebApi.Services.AvailabilitySearch
{
    public interface IBookingResponseService
    {
        EdoContracts.Accommodations.Booking Create(Common.Models.Bookings.BookingOrder bookingOrder);

        EdoContracts.Accommodations.Booking CreateEmpty(string bookingReferenceCode);
    }
}