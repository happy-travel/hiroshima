﻿using System;
using System.Collections.Generic;
using System.Linq;
using HappyTravel.EdoContracts.General;
using HappyTravel.Hiroshima.Common.Infrastructure.Extensions;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms;
using HappyTravel.Hiroshima.DirectContracts.Models;
using HappyTravel.Money.Enums;
using HappyTravel.Money.Helpers;
using HappyTravel.Money.Models;

namespace HappyTravel.Hiroshima.DirectContracts.Services.Availability
{
    public class PaymentDetailsService : IPaymentDetailsService
    {
        public PaymentDetails Create(DateTime checkInDate, DateTime checkOutDate, List<RoomRate> rates, List<RoomPromotionalOffer> promotionalOffers,
            string languageCode)
        {
            var currency = rates.First().Currency;

            var seasonRangesWithPrice =
                rates.SelectMany(rate => rate.Season.SeasonRanges.Select(seasonRange => (seasonRange.StartDate, seasonRange.EndDate, new MoneyAmount(rate.Price, currency)))).ToList();

            var seasonPrices = CalculatePrices(checkInDate, checkOutDate, seasonRangesWithPrice, currency, promotionalOffers, languageCode);

            var totalPrice = new MoneyAmount(seasonPrices.Sum(priceDetails => priceDetails.TotalAmount.Amount), currency);
            var discountAmount = new MoneyAmount(seasonPrices.Sum(seasonPrice => GetDiscountAmount(seasonPrice.TotalAmount, seasonPrice.Discount, currency).Amount), currency);
            var discountPercent = GetDiscountPercent(totalPrice, discountAmount);

            var remarks = RetrievePaymentRemarks(rates);

            return new PaymentDetails(totalPrice, discountPercent, seasonPrices, remarks);
        }


        private List<SeasonPriceDetails> CalculatePrices(DateTime checkInDate, DateTime checkOutDate,
            List<(DateTime StartDate, DateTime EndDate, MoneyAmount rateAmount)> seasonRangesWithPrice, Currencies currency,
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
                    CalculateSeasonPrice(startDate, endDate, currentSeason.rateAmount, currency, promotionalOffers, languageCode);

                if (nights != 0)
                    seasonPrices.Add(new SeasonPriceDetails(startDate, endDate, currentSeason.rateAmount, nights, seasonPriceTotal, seasonPriceWithDiscountTotal,
                        discountPercentTotal, dailyPrices));

                startDate = endDate;
            }

            return seasonPrices;
        }


        private (MoneyAmount totalAmount, MoneyAmount amountWithDiscountTotal, Discount discount, List<SeasonDailyPrice> dailyPrices)
            CalculateSeasonPrice(DateTime seasonStartDate, DateTime seasonEndDate, MoneyAmount dailyAmount, Currencies currency, List<RoomPromotionalOffer> promotionalOffers, string languageCode)
        {
            var fromDate = seasonStartDate;
            var seasonAmountWithDiscountTotal = new MoneyAmount(0, currency);
            var discountAmountTotal = new MoneyAmount(0, currency);

            var dailyPrices = new List<SeasonDailyPrice>();
            while (fromDate < seasonEndDate)
            {
                var toDate = fromDate.AddDays(1);
                var discount = GetDiscountForDay(fromDate, promotionalOffers, languageCode);

                var discountAmount = GetDiscountAmount(dailyAmount, discount, currency);
                var dailyAmountWithDiscount = dailyAmount - discountAmount;

                dailyPrices.Add(new SeasonDailyPrice(fromDate, toDate, dailyAmount, dailyAmountWithDiscount, discount));

                discountAmountTotal += discountAmount;
                seasonAmountWithDiscountTotal += dailyAmountWithDiscount;

                fromDate = toDate;
            }

            var seasonPriceTotal = new MoneyAmount(dailyAmount.Amount * (seasonEndDate.Date - seasonStartDate.Date).Days, currency);
            var discountPercentTotal = GetDiscountPercent(seasonPriceTotal, discountAmountTotal);

            return (seasonPriceTotal, seasonAmountWithDiscountTotal, discountPercentTotal, dailyPrices);
        }


        private Discount GetDiscountForDay(DateTime day, List<RoomPromotionalOffer> promotionalOffers, string languageCode)
        {
            foreach (var promotionalOffer in promotionalOffers)
            {
                if (promotionalOffer.ValidFromDate <= day && day <= promotionalOffer.ValidToDate)
                {
                    var description = string.Empty;
                    promotionalOffer.Description?.GetValue<MultiLanguage<string>>().TryGetValueOrDefault(languageCode, out description);

                    return new Discount(promotionalOffer.DiscountPercent, description);
                }
            }

            return new Discount();
        }


        private List<string> RetrievePaymentRemarks(List<RoomRate> rates)
            => rates.Where(rate => rate.Description.IsNotEmpty()).Select(rateDetails => rateDetails.Description.GetFirstValue()).ToList();


        private static MoneyAmount GetDiscountAmount(MoneyAmount moneyAmount, Discount discount, Currencies currency)
            => new MoneyAmount(MoneyRounder.Truncate(moneyAmount.Amount / 100 * discount.Percent, currency), currency);


        private static Discount GetDiscountPercent(MoneyAmount totalMoneyAmount, MoneyAmount moneyAmountWithDiscount, string description = null)
            => new Discount(Math.Truncate(moneyAmountWithDiscount.Amount * 100 / totalMoneyAmount.Amount), description);
    }
}