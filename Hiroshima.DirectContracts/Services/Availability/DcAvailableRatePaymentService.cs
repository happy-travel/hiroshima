﻿using System;
using System.Collections.Generic;
using System.Linq;
using HappyTravel.Money.Enums;
using HappyTravel.Money.Helpers;
using Hiroshima.DbData.Models.Rooms;
using Hiroshima.DirectContracts.Models;

namespace Hiroshima.DirectContracts.Services.Availability
{
    public class DcAvailableRatePaymentService: IDcAvailableRatePaymentService
    {
        public PaymentDetails CreatePaymentDetails(DateTime checkInDate, DateTime checkOutDate,
            List<RoomRate> roomRates, RoomPromotionalOffer roomPromotionalOffer = null)
        {
            List<string> paymentDetails = null;
            var currency = Currencies.NotSpecified;

            if (roomPromotionalOffer != null)
            {
                foreach (var roomRate in roomRates)
                {
                    if (currency == Currencies.NotSpecified)
                        currency = GetCurrency(roomRate.CurrencyCode);
                    roomRate.Price = ApplyDiscount(roomRate.Price, roomPromotionalOffer.DiscountPercent, currency);
                }

                paymentDetails = new List<string> {roomPromotionalOffer.GetDetailsFromFirstLanguage()};
            }

            var seasonPrices = GetSeasonPrices(checkInDate, checkOutDate,
                roomRates.Select(rr => (rr.StartDate, rr.EndDate, rr.Price)).ToList());
            var dailyPrices = GetDailyPrices(seasonPrices);
            var totalPrice = seasonPrices.Sum(sp => sp.TotalPrice);
            
            return new PaymentDetails
            {
                Currency = currency,
                Details = paymentDetails,
                DailyPrices = dailyPrices,
                SeasonPrices = seasonPrices,
                TotalPrice = totalPrice
            };
        }


        private Currencies GetCurrency(string currencyCode)
            => Enum.Parse<Currencies>(currencyCode);
        
        
        private decimal ApplyDiscount(decimal originalPrice, double discountPercent, Currencies currency)
        {
            return originalPrice - MoneyCeiler.Ceil(originalPrice / 100 * Convert.ToDecimal(discountPercent), currency);
        }

        
        private List<decimal> GetDailyPrices(List<SeasonPriceDetails> seasonPrices)
        {
            var dailyPrices = new List<decimal>();
            foreach (var seasonPrice in seasonPrices)
            {
                var nights = (seasonPrice.EndDate - seasonPrice.StartDate).Days;

                for (var i = 0; i < nights; i++)
                {
                    dailyPrices.Add(seasonPrice.RatePrice);
                }
            }

            return dailyPrices;
        }


        private List<SeasonPriceDetails> GetSeasonPrices(DateTime checkInDate, DateTime checkOutDate,
            List<(DateTime StartDate, DateTime EndDate, decimal ratePrice)> seasons)
        {
            if (checkInDate >= checkOutDate || !seasons.Any())
                return new List<SeasonPriceDetails>();

            seasons = seasons.OrderBy(i => i.EndDate).ToList();

            var seasonPrices = new List<SeasonPriceDetails>(seasons.Count);

            var startDate = checkInDate;
            for (var i = 0; i < seasons.Count; i++)
            {
                var currentSeason = seasons[i];
                var nextSeasonIndex = i + 1;
                var endDate = nextSeasonIndex < seasons.Count
                    ? seasons[i + 1].StartDate
                    : checkOutDate;
                var nights = (endDate - startDate).Days;
                
                if (nights != 0)
                    seasonPrices.Add(new SeasonPriceDetails
                    {
                        StartDate = startDate,
                        EndDate = endDate,
                        Nights = nights,
                        RatePrice = currentSeason.ratePrice,
                        TotalPrice = nights * currentSeason.ratePrice
                    });

                startDate = endDate;
            }

            return seasonPrices;
        }
    }
}