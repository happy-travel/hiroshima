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
        ThreeStars = 16,
        FourStars = 32,
        FiveStars = 64
    }
}
