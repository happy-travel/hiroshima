using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Hiroshima.DbData.Models.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TextualDescriptionTypes
    {
        Unspecified = 0,
        Exterior = 1,
        General = 2,
        Lobby = 3,
        Position = 4,
        Restaurant = 5,
        Room = 6
    }
}
