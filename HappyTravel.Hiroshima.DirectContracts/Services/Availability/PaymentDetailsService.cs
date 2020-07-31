using System;
using System.Collections.Generic;
using System.Linq;
using HappyTravel.Hiroshima.Common.Infrastructure.Extensions;
using HappyTravel.Hiroshima.Data.Models.Rooms;
using HappyTravel.Hiroshima.DirectContracts.Models;
using HappyTravel.Money.Enums;
using HappyTravel.Money.Helpers;

namespace HappyTravel.Hiroshima.DirectContracts.Services.Availability
{
    public class PaymentDetailsService: IPaymentDetailsService
    {
        public PaymentDetails Create(DateTime checkInDate, DateTime checkOutDate,
            List<RateDetails> rateDetails, List<RoomPromotionalOffer> roomPromotionalOffers)
        {
            var currency = rateDetails.First().Currency;
            var seasonPrices = GetSeasonPrices(checkInDate, checkOutDate,
                rateDetails.Select(rd => (rd.SeasonStartDate, rd.SeasonEndDate, rd.Price)).ToList(), currency, roomPromotionalOffers);
            var dailyPrices = GetDailyPrices(seasonPrices);
            var totalPrice = seasonPrices.Sum(sp => sp.TotalPrice);
            var details = CreatePaymentDetails(rateDetails);
            
            return new PaymentDetails
            {
                Currency = currency,
                SeasonPrices = seasonPrices,
                DailyPrices = dailyPrices,
                TotalPrice = totalPrice,
                Details = details
            };
        }


        private List<string> CreatePaymentDetails(List<RateDetails> rates)
            => rates.Select(rateDetails => rateDetails.Details.GetFirstValue()).ToList();
            
    
        private static decimal ApplyDiscount(decimal originalPrice, double discountPercent, Currencies currency) 
            => originalPrice - MoneyRounder.Truncate( originalPrice / 100 * Convert.ToDecimal(discountPercent), currency);
        
        
        private List<decimal> GetDailyPrices(List<SeasonPriceDetails> seasonPrices)
        {
            var dailyPrices = new List<decimal>();
            foreach (var seasonPrice in seasonPrices)
            {
                var nights = (seasonPrice.EndDate - seasonPrice.StartDate).Days;

                for (var i = 0; i < nights; i++)
                    dailyPrices.Add(seasonPrice.RatePrice);
            }

            return dailyPrices;
        }


        private List<SeasonPriceDetails> GetSeasonPrices(DateTime checkInDate, DateTime checkOutDate,
            List<(DateTime StartDate, DateTime EndDate, decimal ratePrice)> seasons, Currencies currency, List<RoomPromotionalOffer> promotionalOffers)
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
                    ? seasons[nextSeasonIndex].StartDate
                    : checkOutDate;
                var nights = (endDate - startDate).Days;
                var (price, priceDetails) = CalculatePrice(startDate, endDate, currentSeason.ratePrice);
                
                if (nights != 0)
                    seasonPrices.Add(new SeasonPriceDetails
                    {
                        StartDate = startDate,
                        EndDate = endDate,
                        NumberOfNights = nights,
                        RatePrice = currentSeason.ratePrice,
                        TotalPrice = price,
                        Details = priceDetails
                    });

                startDate = endDate;
            }

            return seasonPrices;

            
            (decimal price, List<string> priceDetails) CalculatePrice(DateTime seasonStartDate, DateTime seasonEndDate, decimal seasonPrice)
            {
                var fromDate = seasonStartDate;
                var seasonPriceWithDiscount = 0m;
                var priceDetails = new List<string>();
                while (fromDate < seasonEndDate)
                {
                    var (price, details) = AddDiscount(fromDate);
                    if (!string.IsNullOrEmpty(details))
                        priceDetails.Add(details);
                    
                    seasonPriceWithDiscount += price;
                    fromDate = fromDate.AddDays(1);
                }

                return (seasonPriceWithDiscount, priceDetails);
                
                
                (decimal price, string details) AddDiscount(DateTime day)
                {
                    foreach (var promotionalOffer in promotionalOffers)
                    {
                        if (promotionalOffer.ValidFromDate <= day &&
                            day <= promotionalOffer.ValidToDate)
                        {
                            seasonPrice = ApplyDiscount(seasonPrice, promotionalOffer.DiscountPercent, currency);
                            return (seasonPrice, promotionalOffer.Details.GetFirstValue());
                        }
                    }

                    return (seasonPrice, string.Empty);
                }
            }
        }
    }
}