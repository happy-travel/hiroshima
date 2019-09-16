using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Hiroshima.DbData.Models.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    [Flags]
    public enum PropertyType
    {
        Any = 0,
        Hotels = 1,
        Apartments = 2
    }
}
