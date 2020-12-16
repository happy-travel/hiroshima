using System;
using System.Collections.Generic;
using System.Linq;
using HappyTravel.EdoContracts.General;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms;
using HappyTravel.Hiroshima.Common.Models.Availabilities;
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

            var totalAmount = new MoneyAmount(seasonPrices.Sum(priceDetails => priceDetails.TotalAmount.Amount), currency);
            var totalAmountWithDiscount = new MoneyAmount(seasonPrices.Sum(seasonPrice => seasonPrice.TotalAmountWithDiscount.Amount), currency);
            var discountPercent = GetDiscount(totalAmount, totalAmountWithDiscount);

            var remarks = RetrievePaymentRemarks(rates, languageCode);

            return new PaymentDetails(totalAmount, discountPercent, seasonPrices, remarks);
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
                var (totalSeasonAmount, totalSeasonAmountWithDiscount, discount, dailyPrices) =
                    CalculateSeasonPrice(startDate, endDate, currentSeason.rateAmount, currency, promotionalOffers, languageCode);

                if (nights != 0)
                    seasonPrices.Add(new SeasonPriceDetails(startDate, endDate, currentSeason.rateAmount, nights, totalSeasonAmount, totalSeasonAmountWithDiscount,
                        discount, dailyPrices));

                startDate = endDate;
            }

            return seasonPrices;
        }


        private (MoneyAmount totalAmount, MoneyAmount amountWithDiscountTotal, Discount discount, List<SeasonDailyPrice> dailyPrices)
            CalculateSeasonPrice(DateTime seasonStartDate, DateTime seasonEndDate, MoneyAmount dailyAmount, Currencies currency, List<RoomPromotionalOffer> promotionalOffers, string languageCode)
        {
            var fromDate = seasonStartDate;
            var seasonAmountWithDiscountTotal = new MoneyAmount(0, currency);
            
            var dailyPrices = new List<SeasonDailyPrice>();
            while (fromDate < seasonEndDate)
            {
                var toDate = fromDate.AddDays(1);
                var discount = GetDiscountForDay(fromDate, promotionalOffers, languageCode);

                var discountAmount = GetDiscountAmount(dailyAmount, discount, currency);
                var dailyAmountWithDiscount = dailyAmount - discountAmount;

                dailyPrices.Add(new SeasonDailyPrice(fromDate, toDate, dailyAmount, dailyAmountWithDiscount, discount));

                seasonAmountWithDiscountTotal += dailyAmountWithDiscount;

                fromDate = toDate;
            }

            var totalSeasonAmount = new MoneyAmount(dailyAmount.Amount * (seasonEndDate.Date - seasonStartDate.Date).Days, currency);
            var discountPercentTotal = GetDiscount(totalSeasonAmount, seasonAmountWithDiscountTotal);

            return (totalSeasonAmount, seasonAmountWithDiscountTotal, discountPercentTotal, dailyPrices);
        }


        private Discount GetDiscountForDay(DateTime day, List<RoomPromotionalOffer> promotionalOffers, string languageCode)
        {
            foreach (var promotionalOffer in promotionalOffers)
            {
                if (promotionalOffer.ValidFromDate <= day && day <= promotionalOffer.ValidToDate)
                {
                    promotionalOffer.Description.TryGetValueOrDefault(languageCode, out string description);

                    return new Discount(promotionalOffer.DiscountPercent, description);
                }
            }

            return new Discount();
        }


        private List<string> RetrievePaymentRemarks(List<RoomRate> rates, string languageCode)
            => rates.Where(rate => rate.Description.GetAll().Any()).Select(rateDetails =>
            {
                rateDetails.Description.TryGetValue(languageCode, out var description);
                return description;
            }).ToList();

        
        private static MoneyAmount GetDiscountAmount(MoneyAmount moneyAmount, Discount discount, Currencies currency)
            => new MoneyAmount(MoneyRounder.Truncate(moneyAmount.Amount / 100 * (decimal)discount.Percent, currency), currency);


        private static Discount GetDiscount(MoneyAmount totalMoneyAmount, MoneyAmount moneyAmountWithDiscount, string? description = null)
            => new Discount(Convert.ToDouble(100 - Math.Truncate(moneyAmountWithDiscount.Amount * 100 / totalMoneyAmount.Amount)), description);
    }
}