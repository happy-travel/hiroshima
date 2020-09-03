using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.CancellationPolicies;
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.DirectContracts.Models;
using Microsoft.EntityFrameworkCore;

namespace HappyTravel.Hiroshima.DirectContracts.Services.Availability
{
    public class RateAvailabilityService : IRateAvailabilityService
    {
        public RateAvailabilityService(DirectContractsDbContext dbContext, ICancellationPolicyService cancellationPolicyService,
            IPaymentDetailsService paymentDetailsService)
        {
            _dbContext = dbContext;
            _cancellationPolicyService = cancellationPolicyService;
            _paymentDetailsService = paymentDetailsService;
        }


        public async Task<List<RateOffer>> GetAvailableRates(IEnumerable<Room> rooms, DateTime checkInDate, DateTime checkOutDate, string languageCode)
        {
            var roomsDictionary = rooms.ToDictionary(r => r.Id);
            var roomIds = roomsDictionary.Keys;
            var rates = await GetRateDetails(roomIds.ToList(), checkInDate, checkOutDate, languageCode);
            var roomsPromotionalOffers = await GetPromotionalOffers(roomIds, checkInDate, checkOutDate, languageCode);

            var groupedPromotionalOffers = roomsPromotionalOffers.GroupBy(po => po.RoomId).ToDictionary(g => g.Key, g => g.ToList());

            var cancellationPolicies = (await GetCancellationPolicies(roomIds, checkInDate)).ToDictionary(rcp => rcp.RoomId);

            var availableRateOffers = new List<RateOffer>();
            foreach (var roomRateGroup in rates.GroupBy(rateDetails => rateDetails.RoomId))
            {
                var roomRates = roomRateGroup.ToList();
                if (!roomRates.Any())
                    continue;

                var roomId = roomRateGroup.Key;
                var room = roomsDictionary[roomId];

                var roomPromotionalOffers = groupedPromotionalOffers[roomId];

                var paymentDetails = _paymentDetailsService.Create(checkInDate, checkOutDate, roomRates, roomPromotionalOffers);

                var roomCancellationPolicies = cancellationPolicies[roomId];

                var cancellationPolicyDetails = _cancellationPolicyService.Get(roomCancellationPolicies, checkInDate, paymentDetails);

                var firstRate = roomRateGroup.First();

                availableRateOffers.Add(new RateOffer(
                    room,
                    paymentDetails,
                    cancellationPolicyDetails,
                    firstRate.MealPlan,
                    firstRate.BoardBasis,
                    new List<TaxDetails>(),
                    new List<string>()));
            }

            return availableRateOffers;
        }


        private async Task<List<RateDetails>> GetRateDetails(List<int> roomIds, DateTime checkInDate, DateTime checkOutDate, string languageCode)
        {
            checkInDate = checkInDate.Date;
            var seasonsAndSeasonRanges = _dbContext.Seasons.Join(_dbContext.SeasonRanges, season => season.Id, seasonRange => seasonRange.SeasonId,
                    (season, seasonRange) => new 
                    {
                        Season = season,
                        SeasonRange = seasonRange
                    })
                .Where(seasonAndSeasonRange
                    => !(seasonAndSeasonRange.SeasonRange.EndDate < checkInDate || checkOutDate < seasonAndSeasonRange.SeasonRange.StartDate));

            return await _dbContext.RoomRates.Join(seasonsAndSeasonRanges, rate => rate.SeasonId, seasonAndSeasonRange => seasonAndSeasonRange.Season.Id,
                    (rate, seasonAndSeasonRange) => new {rate, seasonAndSeasonRange.Season, seasonAndSeasonRange.SeasonRange})
                .Where(rateAndSeasonAndSeasonRange => roomIds.Contains(rateAndSeasonAndSeasonRange.rate.RoomId))
                .Select(rateAndSeasonAndSeasonRange => new RateDetails
                {
                    RateId = rateAndSeasonAndSeasonRange.rate.Id,
                    Details = DirectContractsDbContext.GetLangFromJsonb(rateAndSeasonAndSeasonRange.rate.Details, languageCode),
                    Price = rateAndSeasonAndSeasonRange.rate.Price,
                    BoardBasis = rateAndSeasonAndSeasonRange.rate.BoardBasis,
                    Currency = rateAndSeasonAndSeasonRange.rate.Currency,
                    MealPlan = rateAndSeasonAndSeasonRange.rate.MealPlan,
                    RoomId = rateAndSeasonAndSeasonRange.rate.RoomId,
                    SeasonId = rateAndSeasonAndSeasonRange.Season.Id,
                    SeasonName = rateAndSeasonAndSeasonRange.Season.Name,
                    SeasonStartDate = rateAndSeasonAndSeasonRange.SeasonRange.StartDate,
                    SeasonEndDate = rateAndSeasonAndSeasonRange.SeasonRange.EndDate,
                    SeasonContractId = rateAndSeasonAndSeasonRange.Season.Id
                })
                .ToListAsync();
           }


        private async Task<List<RoomPromotionalOffer>> GetPromotionalOffers(IEnumerable<int> roomIds, DateTime checkInDate, DateTime checkOutDate,
            string languageCode)
        {
            checkInDate = checkInDate.Date;
            var dateNow = DateTime.UtcNow.Date;
            return await _dbContext.RoomPromotionalOffers
                .Where(offer => roomIds.Contains(offer.RoomId) && dateNow <= offer.BookByDate &&
                    !(offer.ValidToDate < checkInDate || checkOutDate < offer.ValidFromDate))
                .Select(offer => new RoomPromotionalOffer
                {
                    Id = offer.Id,
                    ValidFromDate = offer.ValidFromDate,
                    ValidToDate = offer.ValidToDate,
                    BookingCode = offer.BookingCode,
                    DiscountPercent = offer.DiscountPercent,
                    RoomId = offer.RoomId,
                    BookByDate = offer.BookByDate,
                    Details = DirectContractsDbContext.GetLangFromJsonb(offer.Details, languageCode)
                })
                .ToListAsync();
        }


        private async Task<List<RoomCancellationPolicy>> GetCancellationPolicies(IEnumerable<int> roomIds, DateTime checkInDate)
        {
            var seasonsAndSeasonRanges = _dbContext.Seasons.Join(_dbContext.SeasonRanges, season => season.Id, seasonRange => seasonRange.SeasonId,
                    (season, seasonRange) => new 
                    {
                        Season = season,
                        SeasonRange = seasonRange
                    })
                .Where(seasonAndSeasonRange => seasonAndSeasonRange.SeasonRange.StartDate.Date <= checkInDate &&
                    checkInDate <= seasonAndSeasonRange.SeasonRange.EndDate.Date);

            return await _dbContext.RoomCancellationPolicies
                .Join(seasonsAndSeasonRanges, roomCancellationPolicy => roomCancellationPolicy.SeasonId, seasonAndSeasonRange => seasonAndSeasonRange.Season.Id,
                    (cancellationPolicy, seasonAndSeasonRange) => new {cancellationPolicy, seasonAndSeasonRange.Season, seasonAndSeasonRange.SeasonRange})
                .Where(cancellationPolicyAndRoomAndSeasonRange => roomIds.Contains(cancellationPolicyAndRoomAndSeasonRange.cancellationPolicy.RoomId))
                .Select(cancellationPolicyAndRoomAndSeasonRange => cancellationPolicyAndRoomAndSeasonRange.cancellationPolicy)
                .ToListAsync();
        }


        private readonly IPaymentDetailsService _paymentDetailsService;
        private readonly ICancellationPolicyService _cancellationPolicyService;
        private readonly DirectContractsDbContext _dbContext;
    }
}