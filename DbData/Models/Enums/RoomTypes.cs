using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Hiroshima.DbData.Models.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum RoomTypes
    {
        NotSpecified = 0,
        Single = 1,
        TwinOrSingle = 2,
        Twin = 3,
        Double = 4,
        Triple = 5,
        Quadruple = 6,
        /// <summary>
        /// Family Room (2 adult + 2 extra bed)
        /// </summary>
        Family = 7
    }
}
