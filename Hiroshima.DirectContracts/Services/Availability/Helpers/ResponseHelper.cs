using System;
using System.Collections.Generic;
using System.Linq;
using Hiroshima.DirectContracts.Models;

namespace Hiroshima.DirectContracts.Services.Availability.Helpers
{
    public static class ResponseHelper
    {
        public static Price GetPrice(List<SeasonPrice> seasons, DateTime checkInDate, DateTime checkOutDate)
        {
            if (checkInDate >= checkOutDate)
                throw new ArgumentException(
                    $"{nameof(checkInDate)}({checkInDate}) must not be more than {nameof(checkOutDate)}(checkOutDate)");

            var response = new Price();
            if (!seasons.Any())
                return response;
            
            var seasonPrices = response.SeasonsPrices;
            var day = checkInDate;
            for (var i = 0; i < seasons.Count; i++)
            {
                var currentSeason = seasons[i];
                var seasonPrice = new SeasonPrice();
                var seasonStarted = false;
                while (day < checkOutDate)
                {
                    var nextDay = day.AddDays(1);
                    if (SeasonContainsDay(currentSeason, nextDay))
                    {
                        if (!seasonStarted)
                        {
                            seasonPrice.StartDate = i > 0 ? nextDay : day;
                            seasonStarted = true;
                        }
                        seasonPrice.Price += currentSeason.Price;
                    }
                    else
                    {
                        seasonPrice.EndDate = day;
                        break;
                    }
                    day = nextDay;
                }
                if (seasonPrice.EndDate.Equals(default))
                    seasonPrice.EndDate= day;
                seasonPrices.Add(seasonPrice);
            }

            return response;
        }


        private static bool SeasonContainsDay(SeasonPrice season, in DateTime day)
        {
            return day.Date >= season.StartDate.Date &&
                   day.Date <= season.EndDate.Date;
        }
    }
}
