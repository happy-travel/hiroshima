using System;
using System.Collections.Generic;
using System.Linq;
using HappyTravel.EdoContracts.Accommodations.Enums;
using HappyTravel.Hiroshima.Common.Infrastructure.Extensions;
using HappyTravel.Hiroshima.Common.Models.Enums;

namespace HappyTravel.Hiroshima.Common.Infrastructure.Utilities
{
    public static class AccommodationRatingMapper
    {
        public static AccommodationRatings GetRating(int stars) => stars switch
        {
            0 => AccommodationRatings.NotRated,
            1 => AccommodationRatings.OneStar,
            2 => AccommodationRatings.TwoStars,
            3 => AccommodationRatings.ThreeStars,
            4 => AccommodationRatings.FourStars,
            5 => AccommodationRatings.FiveStars,
            _ => AccommodationRatings.Unknown
        };


        public static IEnumerable<AccommodationRating> GetStars(AccommodationRatings ratings)
        {
            if (ratings == AccommodationRatings.Unknown || ratings == 0)
                return Enum.GetValues(typeof(AccommodationRating)).Cast<AccommodationRating>();

            return ratings
                .GetFlags<AccommodationRatings>()
                .Select(rating => rating switch
                {
                    AccommodationRatings.NotRated => AccommodationRating.NotRated,
                    AccommodationRatings.OneStar => AccommodationRating.OneStar,
                    AccommodationRatings.TwoStars => AccommodationRating.TwoStars,
                    AccommodationRatings.ThreeStars => AccommodationRating.ThreeStars,
                    AccommodationRatings.FourStars => AccommodationRating.FourStars,
                    AccommodationRatings.FiveStars => AccommodationRating.FiveStars,
                    _ => throw new ArgumentException($"Invalid value {rating}", nameof(rating))
                });
        }
    }
}