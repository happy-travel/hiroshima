using System;
using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using Hiroshima.DirectContracts.Models.Internal;

namespace Hiroshima.DirectContracts.Services.Availability.Helpers
{
    public static class PriceHelper
    {
        public static Result<List<SeasonDailyPrice>> GetSeasonDailyPrices(List<Season> seasons, DateTime checkInDate, DateTime checkOutDate)
        {
            if (checkInDate >= checkOutDate)
                return Result.Failure<List<SeasonDailyPrice>>(
                    $"{nameof(checkInDate)}({checkInDate}) must not be more than {nameof(checkOutDate)}(checkOutDate)");
            
            if (!seasons.Any())
                return Result.Failure<List<SeasonDailyPrice>>($"{nameof(seasons)} are empty");

            seasons = seasons.OrderBy(i => i.EndDate).ToList();

            var dailyPrices = new List<SeasonDailyPrice>(seasons.Count);

            var day = checkInDate;

            for (var i = 0; i < seasons.Count; i++)
            {
                var currentSeason = seasons[i];
                var dailyPrice = new SeasonDailyPrice();
                var seasonStarted = false;

                while (day < checkOutDate)
                {
                    var nextDay = day.AddDays(1);
                    if (SeasonContainsDay(currentSeason, nextDay))
                    {
                        if (!seasonStarted)
                        {
                            dailyPrice.StartDate = i > 0 ? nextDay : day;
                            seasonStarted = true;
                        }
                        dailyPrice.TotalPrice += currentSeason.NightPrice;
                    }
                    else
                    {
                        dailyPrice.EndDate = day;
                        break;
                    }
                    day = nextDay;
                }

                if (dailyPrice.EndDate.Equals(default))
                    dailyPrice.EndDate = day;

                dailyPrice.SeasonName = currentSeason.SeasonName;
                dailyPrices.Add(dailyPrice);
            }

            return Result.Success(dailyPrices);
        }


        private static bool SeasonContainsDay(Season season, in DateTime day)
        {
            return day.Date >= season.StartDate.Date &&
                   day.Date <= season.EndDate.Date;
        }


        private static  readonly List<SeasonDailyPrice> EmptyDailyPrices = new List<SeasonDailyPrice>(0);
    }
}
