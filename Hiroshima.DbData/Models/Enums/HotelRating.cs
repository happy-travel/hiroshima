using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Hiroshima.DbData.Models.Enums
{
    [Flags]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum HotelRating
    {
        Unknown = 0,
        NotRated = 1,
        OneStar = 2,
        TwoStars = 3,
        ThreeStars = 4,
        FourStars = 5,
        FiveStars = 6
    }
}
