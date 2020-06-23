using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HappyTravel.Money.Enums;
using HappyTravel.Money.Helpers;
using Hiroshima.DbData;
using Hiroshima.DbData.Extensions;
using Hiroshima.DbData.Models.Rooms;
using Hiroshima.DbData.Models.Rooms.CancellationPolicies;
using Hiroshima.DirectContracts.Models;

namespace Hiroshima.DirectContracts.Services.Availability
{
    public class DcRateAvailabilityService : IDcRateAvailabilityService
    {
        public DcRateAvailabilityService(DcDbContext dbContext, IDcCancellationPoliciesService cancellationPoliciesService)
        {
            _dbContext = dbContext;
            _cancellationPoliciesService = cancellationPoliciesService;
        }


        public async Task<List<AvailableRate>> GetAvailableRates(IEnumerable<Room> rooms, DateTime checkInDate, DateTime checkOutDate, string languageCode)
        {
            var roomsDictionary = rooms.ToDictionary(r => r.Id);
            var roomIds = roomsDictionary.Keys;
            var rates = await _dbContext.GetRates(roomIds, checkInDate, checkOutDate, languageCode);
            var promotionalOffers =
                (await _dbContext.GetPromotionalOffers(roomIds, checkInDate, languageCode)).ToDictionary(rpo =>
                    rpo.RoomId);
            var cancellationPolicies = (await _dbContext.GetCancellationPolicies(roomIds, checkInDate)).ToDictionary(rcp=>
                rcp.RoomId);
            
            var availableRates = new List<AvailableRate>();
            foreach (var roomRateGroup in rates.GroupBy(rr => rr.RoomId))
            {
                var roomRates = roomRateGroup.ToList();
                if (!roomRates.Any())
                    continue;

                var roomId = roomRateGroup.Key;
                var room = roomsDictionary[roomId];
                
                var promotionalOffer = GetPromotionalOffer(promotionalOffers, roomId);
                var paymentDetails = CreatePaymentDetails(checkInDate, checkOutDate, roomRates, promotionalOffer);

                var roomCancellationPolicies = GetRoomCancellationPolicies(cancellationPolicies, roomId);
                
                var cancellationPolicyDetails =
                    _cancellationPoliciesService.Get(roomCancellationPolicies, checkInDate, paymentDetails);
                
                availableRates.Add( new AvailableRate
                {
                    Room = room,
                    PaymentDetails = paymentDetails,
                    CancellationPolicies = cancellationPolicyDetails
                });
            }

            return availableRates;
        }

        
        private PaymentDetails CreatePaymentDetails(DateTime checkInDate, DateTime checkOutDate,
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
            
            return new PaymentDetails
            {
                Currency = currency,
                Details = paymentDetails,
                SeasonPrices = seasonPrices,
                DailyPrices = GetDailyPrices(seasonPrices),
                TotalPrice = seasonPrices.Sum(sp => sp.TotalPrice)
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


        private RoomCancellationPolicy GetRoomCancellationPolicies(Dictionary<int, RoomCancellationPolicy> roomCancellationPolicies, int roomId)
        {
            roomCancellationPolicies.TryGetValue(roomId, out var cancellationPolicy);
            return cancellationPolicy;
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

        
        private readonly IDcCancellationPoliciesService _cancellationPoliciesService;
        private readonly DcDbContext _dbContext;
    }
}