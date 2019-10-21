using System;
using System.Collections.Generic;
using System.Text;

namespace Hiroshima.Common.Models.Enums
{
    public enum AccommodationRating
    {
        Unknown = 1,
        NotRated = 2,
        OneStar = 4,
        TwoStars = 8,
        ThreeStars = 16, // 0x00000010
        FourStars = 32, // 0x00000020
        FiveStars = 64, // 0x00000040
    }
}
