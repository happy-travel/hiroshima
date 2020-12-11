using HappyTravel.Hiroshima.Common.Models.Enums;

namespace HappyTravel.Hiroshima.DirectManager.Models.Webhooks.Bookings
{
    public class BookingStatusData
    {
        public string ReferenceCode { get; set; }
        public BookingStatuses Status { get; set; }
    }
}