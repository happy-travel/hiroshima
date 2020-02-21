using System;
using System.Collections.Generic;
using System.Linq;
using Hiroshima.DbData.Models.Rates;
using Hiroshima.DirectContracts.Models.Internal;

namespace Hiroshima.DirectContracts.Services
{
    public class PricesService : IPricesService
    {
        public List<SeasonPrice> GetSeasonPrices(ICollection<ContractedRate> contractRates,
            ICollection<DiscountRate> discountRates, DateTime checkInDate,
            DateTime checkOutDate)
        {
            if (contractRates == null || !contractRates.Any())
                return EmptyDailyPrices;

            if (discountRates == null || !discountRates.Any())
                return GetSeasonDailyPrices(contractRates.Select(i => (
                            i.Season.Name,
                            i.Season.StartDate,
                            i.Season.EndDate,
                            i.SeasonPrice))
                        .ToList(),
                    checkInDate,
                    checkOutDate);

            var ratesWithDiscount = GetRatesWithDiscount(contractRates, discountRates);
            return GetSeasonDailyPrices(ratesWithDiscount, checkInDate, checkOutDate);
        }


        private List<SeasonPrice> GetSeasonDailyPrices(
            List<(string SeasonName, DateTime StartDate, DateTime EndDate, decimal NightPrice)> seasons,
            DateTime checkInDate, DateTime checkOutDate)
        {
            if (checkInDate >= checkOutDate || !seasons.Any())
                return EmptyDailyPrices;

            seasons = seasons.OrderBy(i => i.EndDate).ToList();

            var dailyPrices = new List<SeasonPrice>(seasons.Count);

            var day = checkInDate;

            for (var i = 0; i < seasons.Count; i++)
            {
                var currentSeason = seasons[i];
                var dailyPrice = new SeasonPrice();
                var seasonStarted = false;

                while (day < checkOutDate)
                {
                    var nextDay = day.AddDays(1);
                    if (SeasonContainsDay(currentSeason.StartDate, currentSeason.EndDate, nextDay))
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

            return dailyPrices;
        }


        private List<(string SeasonName, DateTime StartDate, DateTime EndDate, decimal NightPrice)>
            GetRatesWithDiscount(
                ICollection<ContractedRate> contractRates, ICollection<DiscountRate> discountRates)
        {
            var discountContractedRates =
                new List<(string SeasonName, DateTime StartDate, DateTime EndDate, decimal NightPrice)>(contractRates
                    .Count);
            foreach (var discountRate in discountRates)
            foreach (var contractRate in contractRates)
            {
                var season = contractRate.Season;
                if (season.StartDate.Date <= discountRate.ValidFrom.Date &&
                    discountRate.ValidFrom.Date <= season.EndDate.Date)
                {
                    discountContractedRates.Add((discountRate.BookingCode, discountRate.ValidFrom, discountRate.ValidTo,
                        contractRate.SeasonPrice - contractRate.SeasonPrice / 100 * discountRate.DiscountPercent));
                    break;
                }
            }

            return discountContractedRates;
        }


        private static bool SeasonContainsDay(DateTime startDate, DateTime endDate, in DateTime day)
        {
            return day.Date >= startDate.Date &&
                   day.Date <= endDate.Date;
        }


        private static readonly List<SeasonPrice> EmptyDailyPrices = new List<SeasonPrice>(0);
    }
}