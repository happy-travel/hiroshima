using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Hiroshima.Common.Models.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    [Flags]
    public enum PropertyTypes
    {
        Any = 0,
        Hotels = 1,
        Apartments = 2
    }
}
