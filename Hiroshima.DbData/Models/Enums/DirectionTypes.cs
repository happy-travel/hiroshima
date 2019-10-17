using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Hiroshima.DbData.Models.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DirectionTypes
    {
        PointOfInterest = 0,
        Airport = 1,
        Bus = 2,
        Center = 3,
        Fair = 4,
        Metro = 5,
        Station = 6
    }
}
