using System;
using System.Collections.Generic;
using System.Linq;
using HappyTravel.EdoContracts.Accommodations.Enums;
using HappyTravel.Hiroshima.Common.Infrastructure.Extensions;
using HappyTravel.Hiroshima.Common.Models.Enums;

namespace HappyTravel.Hiroshima.Common.Infrastructure.Utilities
{
    public static class AccommodationRatingConverter
    {
        public static AccommodationRatings Convert(AccommodationStars rating) => rating switch
        {
            AccommodationStars.NotRated => AccommodationRatings.NotRated,
            AccommodationStars.OneStar => AccommodationRatings.OneStar,
            AccommodationStars.TwoStars => AccommodationRatings.TwoStars,
            AccommodationStars.ThreeStars => AccommodationRatings.ThreeStars,
            AccommodationStars.FourStars => AccommodationRatings.FourStars,
            AccommodationStars.FiveStars => AccommodationRatings.FiveStars,
            _ => throw new ArgumentException($"Invalid value {rating}", nameof(rating))
        };


        public static IEnumerable<AccommodationStars> Convert(AccommodationRatings ratings) 
            => ratings
            .GetFlags<AccommodationRatings>()
            .Select(rating => rating switch
            {
                AccommodationRatings.NotRated => AccommodationStars.NotRated,
                AccommodationRatings.OneStar => AccommodationStars.OneStar,
                AccommodationRatings.TwoStars => AccommodationStars.TwoStars,
                AccommodationRatings.ThreeStars => AccommodationStars.ThreeStars,
                AccommodationRatings.FourStars => AccommodationStars.FourStars,
                AccommodationRatings.FiveStars => AccommodationStars.FiveStars,
                _ => throw new ArgumentException($"Invalid value {rating}", nameof(rating))
            });
    }
}