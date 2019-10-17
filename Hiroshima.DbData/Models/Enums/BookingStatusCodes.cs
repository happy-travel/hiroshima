using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Hiroshima.DbData.Models.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
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
