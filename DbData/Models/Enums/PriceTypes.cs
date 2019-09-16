using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Hiroshima.DbData.Models.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PriceTypes
    {
        Room = 1,
        ExtraBed = 2,
        Cot = 3
    }
}
