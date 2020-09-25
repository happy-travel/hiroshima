using System;
using System.Collections.Generic;
using System.Linq;
using HappyTravel.Hiroshima.Common.Infrastructure.Extensions;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms;
using HappyTravel.Hiroshima.DirectContracts.Models;
using HappyTravel.Money.Enums;
using HappyTravel.Money.Helpers;

namespace HappyTravel.Hiroshima.DirectContracts.Services.Availability
{
    public class PaymentDetailsService: IPaymentDetailsService
    {
        public PaymentDetails Create(DateTime checkInDate, DateTime checkOutDate,
            List<RoomRate> rates, List<RoomPromotionalOffer> promotionalOffers)
        {
            var currency = rates.First().Currency;
            
            var seasonRangesWithPrice = rates.SelectMany(rate =>
                rate.Season.SeasonRanges.Select(seasonRange => (seasonRange.StartDate, seasonRange.EndDate, rate.Price))).ToList();
                
            var seasonPrices = CalculateSeasonPrices(checkInDate, checkOutDate, seasonRangesWithPrice, currency, promotionalOffers);
  
            var totalPrice = seasonPrices.Sum(priceDetails => priceDetails.TotalPrice);
            var remarks = RetrievePaymentRemarks(rates);

            return new PaymentDetails(totalPrice, seasonPrices, currency, remarks);
        }


        private List<string> RetrievePaymentRemarks(List<RoomRate> rates)
            => rates.Where(rate => rate.Remarks.NotEmpty()).Select(rateDetails => rateDetails.Remarks.GetFirstValue()).ToList();
            
    
        private static decimal ApplyDiscount(decimal originalPrice, double discountPercent, Currencies currency) 
            => originalPrice - MoneyRounder.Truncate( originalPrice / 100 * Convert.ToDecimal(discountPercent), currency);
        

        private List<SeasonPriceDetails> CalculateSeasonPrices(DateTime checkInDate, DateTime checkOutDate,
            List<(DateTime StartDate, DateTime EndDate, decimal ratePrice)> seasonRangesWithPrice, Currencies currency, List<RoomPromotionalOffer> promotionalOffers)
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
                var endDate = nextSeasonIndex < seasonRangesWithPrice.Count
                    ? seasonRangesWithPrice[nextSeasonIndex].StartDate
                    : checkOutDate;
                var nights = (endDate - startDate).Days;
                var (totalSeasonPrice, appliedDiscounts, priceRemarks) = CalculatePrice(startDate, endDate, currentSeason.ratePrice);
                
                if (nights != 0)
                    seasonPrices.Add(new SeasonPriceDetails(
                        startDate,
                        endDate,
                        currentSeason.ratePrice,
                        nights,
                        totalSeasonPrice,
                        appliedDiscounts,
                        priceRemarks
                    ));

                startDate = endDate;
            }

            return seasonPrices;

            
            (decimal priceWithDiscount, List<double> appliedDiscounts, List<string> priceRemarks) CalculatePrice(DateTime seasonStartDate, DateTime seasonEndDate, decimal seasonPrice)
            {
                var fromDate = seasonStartDate;
                var seasonPriceWithDiscount = 0m;
                var priceRemarks = new List<string>();
                var appliedDiscounts = new List<double>();
                while (fromDate < seasonEndDate)
                {
                    var (priceWithDiscount, discountPercent, priceRemark) = ApplyDiscount(fromDate);
                    if (!string.IsNullOrEmpty(priceRemark))
                        priceRemarks.Add(priceRemark);
                    appliedDiscounts.Add(discountPercent);
                    seasonPriceWithDiscount += priceWithDiscount;
                    fromDate = fromDate.AddDays(1);
                }

                return (seasonPriceWithDiscount, appliedDiscounts, priceRemarks);
                
                
                (decimal price, double discountPercent, string details) ApplyDiscount(DateTime day)
                {
                    foreach (var promotionalOffer in promotionalOffers)
                    {
                        if (promotionalOffer.ValidFromDate <= day &&
                            day <= promotionalOffer.ValidToDate)
                        {
                            var remarks = promotionalOffer.Remarks == null
                                ? string.Empty
                                : promotionalOffer.Remarks.GetFirstValue();
                                
                            var priceWithDiscount = PaymentDetailsService.ApplyDiscount(seasonPrice, promotionalOffer.DiscountPercent, currency);
                            return (priceWithDiscount, promotionalOffer.DiscountPercent, remarks);
                        }
                    }

                    return (seasonPrice, 0, string.Empty);
                }
            }
        }
    }
}