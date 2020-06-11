using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HappyTravel.Money.Enums;
using HappyTravel.Money.Helpers;
using Hiroshima.Common.Models;
using Hiroshima.DbData;
using Hiroshima.DbData.Extensions;
using Hiroshima.DbData.Models.Rooms;
using Hiroshima.DirectContracts.Models.Internal.Response;

namespace Hiroshima.DirectContracts.Services.Availability
{
    public class DirectContractsRateAvailabilityService : IDirectContractsRateAvailabilityService
    {
        public DirectContractsRateAvailabilityService(DirectContractsDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task GetAvailableRates(IEnumerable<Room> rooms, DateTime checkInDate, DateTime checkOutDate)
        {
            var roomsDictionary = rooms.ToDictionary(r => r.Id);
            var rates = await _dbContext.GetRates(roomsDictionary.Keys, checkInDate, checkOutDate);
            var promotionalOffers =
                (await _dbContext.GetPromotionalOffers(roomsDictionary.Keys, checkInDate)).ToDictionary(rpo =>
                    rpo.RoomId);

            var availableRates = new List<AvailableRate>();
            foreach (var roomRateGroup in rates.GroupBy(rr => rr.RoomId))
            {
                var roomRates = roomRateGroup.ToList();
                if (!roomRates.Any())
                    continue;

                var roomId = roomRateGroup.Key;

                var promotionalOffer = GetPromotionalOffer(promotionalOffers, roomId);
                var room = roomsDictionary[roomId];
                var payment = CreatePaymentDetails(checkInDate, checkOutDate, roomRates, promotionalOffer);

                var availableRate = new AvailableRate
                {
                    
                };
                
                availableRates.Add(availableRate);
            }
            
        }

        private PaymentDetails CreatePaymentDetails(DateTime checkInDate, DateTime checkOutDate,
            List<RoomRateData> roomRates, RoomPromotionalOffer roomPromotionalOffer = null)
        {
            var promotionalOfferDetails = new MultiLanguage<string>();
            var currency = Currencies.NotSpecified;

            if (roomPromotionalOffer != null)
            {
                foreach (var roomRate in roomRates)
                {
                    if (currency == Currencies.NotSpecified)
                        currency = GetCurrency(roomRate.CurrencyCode);
                    roomRate.Price = ApplyDiscount(roomRate.Price, roomPromotionalOffer.DiscountPercent, currency);
                }

                promotionalOfferDetails = roomPromotionalOffer.Details;
            }

            var seasonPrices = GetSeasonPrices(checkInDate, checkOutDate,
                roomRates.Select(rr => (rr.StartDate, rr.EndDate, rr.Price)).ToList());
            var dailyPrices = GetDailyPrices(seasonPrices);
            var totalPrice = seasonPrices.Sum(sp => sp.TotalPrice);

            return new PaymentDetails
            {
                Currency = currency,
                Details = promotionalOfferDetails,
                DailyPrices = dailyPrices,
                SeasonPrices = seasonPrices,
                TotalPrice = totalPrice
            };
        }


        private Currencies GetCurrency(string currencyCode)
        {
            return Enum.Parse<Currencies>(currencyCode);
        }


        private decimal ApplyDiscount(decimal originalPrice, double discountPercent, Currencies currency)
        {
            return originalPrice - MoneyCeiler.Ceil(originalPrice / 100 * Convert.ToDecimal(discountPercent), currency);
        }

        
        private RoomPromotionalOffer GetPromotionalOffer(Dictionary<int, RoomPromotionalOffer> roomPromotionalOffers,
            int roomId)
        {
            roomPromotionalOffers.TryGetValue(roomId, out var promotionalOffer);
            return promotionalOffer;
        }


        private List<decimal> GetDailyPrices(List<SeasonPrice> seasonPrices)
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


        private List<SeasonPrice> GetSeasonPrices(DateTime checkInDate, DateTime checkOutDate,
            List<(DateTime StartDate, DateTime EndDate, decimal ratePrice)> seasons)
        {
            if (checkInDate >= checkOutDate || !seasons.Any())
                return new List<SeasonPrice>();

            seasons = seasons.OrderBy(i => i.EndDate).ToList();

            var seasonPrices = new List<SeasonPrice>(seasons.Count);

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
                    seasonPrices.Add(new SeasonPrice
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


        private readonly DirectContractsDbContext _dbContext;
    }
}