namespace Hiroshima.Common.Models.Enums
{
    public enum BookingStatusCodes
    {
        InternalProcessing = 0,
        WaitingForResponse = 1,
        Pending = 2,
        Confirmed = 3,
        Cancelled = 4,
        Rejected = 5,
        Invalid = 6
    }
}
