using System;
using System.Collections.Generic;
using System.Linq;
using HappyTravel.EdoContracts.Accommodations.Enums;
using HappyTravel.Hiroshima.Common.Infrastructure.Extensions;

namespace HappyTravel.Hiroshima.WebApi.Controllers.DirectManager
{
    public static class AccommodationRatingMapper
    {
        public static AccommodationRatings GetRating(int stars)
        {
            switch (stars)
            {
                case 0:
                    return AccommodationRatings.NotRated;
                case 1:
                    return AccommodationRatings.OneStar;
                case 2:
                    return AccommodationRatings.TwoStars;
                case 3:
                    return AccommodationRatings.ThreeStars;
                case 4:
                    return AccommodationRatings.FourStars;
                case 5:
                    return AccommodationRatings.FiveStars;
                default:
                    return AccommodationRatings.Unknown;
            }
        }


        public static IEnumerable<int> GetStars(AccommodationRatings ratings)
        {
            if (ratings == AccommodationRatings.Unknown || ratings == 0)
                return new[] {0, 1, 2, 3, 4, 5};

            return ratings
                .GetFlags<AccommodationRatings>()
                .Select(rating =>
                {
                    switch (rating)
                    {
                        case AccommodationRatings.NotRated:
                            return 0;
                        case AccommodationRatings.OneStar:
                            return 1;
                        case AccommodationRatings.TwoStars:
                            return 2;
                        case AccommodationRatings.ThreeStars:
                            return 3;
                        case AccommodationRatings.FourStars:
                            return 4;
                        case AccommodationRatings.FiveStars:
                            return 5;
                        default:
                            throw new ArgumentException($"Invalid value {rating}", nameof(rating));
                    }
                });
        }
    }
}