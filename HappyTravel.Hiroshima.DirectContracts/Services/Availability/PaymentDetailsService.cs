using System;
using System.Collections.Generic;
using System.Linq;
using HappyTravel.Hiroshima.Common.Infrastructure.Extensions;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms;
using HappyTravel.Hiroshima.DirectContracts.Models;
using HappyTravel.Money.Enums;
using HappyTravel.Money.Helpers;

namespace HappyTravel.Hiroshima.DirectContracts.Services.Availability
{
    public class PaymentDetailsService : IPaymentDetailsService
    {
        public PaymentDetails Create(DateTime checkInDate, DateTime checkOutDate, List<RoomRate> rates, List<RoomPromotionalOffer> promotionalOffers,
            string languageCode)
        {
            var currency = rates.First().Currency;

            var seasonRangesWithPrice =
                rates.SelectMany(rate => rate.Season.SeasonRanges.Select(seasonRange => (seasonRange.StartDate, seasonRange.EndDate, rate.Price))).ToList();

            var seasonPrices = CalculatePrices(checkInDate, checkOutDate, seasonRangesWithPrice, currency, promotionalOffers, languageCode);

            var priceTotal = seasonPrices.Sum(priceDetails => priceDetails.PriceTotal);
            var discountAmount = seasonPrices.Sum(seasonPrice => GetDiscountAmount(seasonPrice.PriceTotal, seasonPrice.DiscountPercentTotal, currency));
            var discountPercent = GetDiscountPercent(priceTotal, discountAmount);

            var remarks = RetrievePaymentRemarks(rates);

            return new PaymentDetails(priceTotal, discountPercent, seasonPrices, currency, remarks);
        }


        private List<SeasonPriceDetails> CalculatePrices(DateTime checkInDate, DateTime checkOutDate,
            List<(DateTime StartDate, DateTime EndDate, decimal ratePrice)> seasonRangesWithPrice, Currencies currency,
            List<RoomPromotionalOffer> promotionalOffers, string languageCode)
        {
            if (checkInDate >= checkOutDate || !seasonRangesWithPrice.Any())
                return new List<SeasonPriceDetails>();

            seasonRangesWithPrice = seasonRangesWithPrice.OrderBy(i => i.EndDate).ToList();

            var seasonPrices = new List<SeasonPriceDetails>(seasonRangesWithPrice.Count);

            var startDate = checkInDate;
            for (var i = 0; i < seasonRangesWithPrice.Count; i++)
            {
                var currentSeason = seasonRangesWithPrice[i];
                var nextSeasonIndex = i + 1;
                var endDate = nextSeasonIndex < seasonRangesWithPrice.Count ? seasonRangesWithPrice[nextSeasonIndex].StartDate : checkOutDate;
                var nights = (endDate - startDate).Days;
                var (seasonPriceTotal, seasonPriceWithDiscountTotal, discountPercentTotal, dailyPrices) =
                    CalculateSeasonPrice(startDate, endDate, currentSeason.ratePrice, currency, promotionalOffers, languageCode);

                if (nights != 0)
                    seasonPrices.Add(new SeasonPriceDetails(startDate, endDate, currentSeason.ratePrice, nights, seasonPriceTotal, seasonPriceWithDiscountTotal,
                        discountPercentTotal, dailyPrices));

                startDate = endDate;
            }

            return seasonPrices;
        }


        private (decimal seasonPriceTotal, decimal seasonPriceWithDiscountTotal, double discountPercent, List<SeasonDailyPrice> dailyPrices)
            CalculateSeasonPrice(DateTime seasonStartDate, DateTime seasonEndDate, decimal dailyPrice, Currencies currency, List<RoomPromotionalOffer> promotionalOffers, string languageCode)
        {
            var fromDate = seasonStartDate;
            var seasonPriceWithDiscountTotal = 0m;
            var discountAmountTotal = 0m;

            var dailyPrices = new List<SeasonDailyPrice>();
            while (fromDate < seasonEndDate)
            {
                var toDate = fromDate.AddDays(1);
                var (discountPercent, discountRemark) = GetDiscountForDay(fromDate, promotionalOffers, languageCode);

                var discountAmount = GetDiscountAmount(dailyPrice, discountPercent, currency);
                var dailyPriceWithDiscount = dailyPrice - discountAmount;

                dailyPrices.Add(new SeasonDailyPrice(fromDate, toDate, dailyPrice, dailyPriceWithDiscount, discountRemark));

                discountAmountTotal += discountAmount;
                seasonPriceWithDiscountTotal += dailyPriceWithDiscount;

                fromDate = toDate;
            }

            var seasonPriceTotal = dailyPrice * (seasonEndDate.Date - seasonStartDate.Date).Days;
            var discountPercentTotal = GetDiscountPercent(seasonPriceTotal, discountAmountTotal);

            return (seasonPriceTotal, seasonPriceWithDiscountTotal, discountPercentTotal, dailyPrices);
        }


        private (double discountPercent, string details) GetDiscountForDay(DateTime day, List<RoomPromotionalOffer> promotionalOffers, string languageCode)
        {
            foreach (var promotionalOffer in promotionalOffers)
            {
                if (promotionalOffer.ValidFromDate <= day && day <= promotionalOffer.ValidToDate)
                {
                    var remarks = string.Empty;
                    promotionalOffer.Remarks?.GetValue<MultiLanguage<string>>().TryGetValueOrDefault(languageCode, out remarks);

                    return (promotionalOffer.DiscountPercent, remarks);
                }
            }

            return (0, string.Empty);
        }


        private List<string> RetrievePaymentRemarks(List<RoomRate> rates)
            => rates.Where(rate => rate.Remarks.IsNotEmpty()).Select(rateDetails => rateDetails.Remarks.GetFirstValue()).ToList();


        private static decimal GetDiscountAmount(decimal price, double discountPercent, Currencies currency)
            => MoneyRounder.Truncate(price / 100 * Convert.ToDecimal(discountPercent), currency);


        private static double GetDiscountPercent(decimal priceTotal, decimal priceWithDiscount)
            => Math.Truncate(Convert.ToDouble(priceWithDiscount * 100 / priceTotal));
    }
}