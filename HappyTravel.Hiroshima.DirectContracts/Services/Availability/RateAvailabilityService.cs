using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.Data.Models.Rooms;
using HappyTravel.Hiroshima.Data.Models.Rooms.CancellationPolicies;
using HappyTravel.Hiroshima.DirectContracts.Models;
using Microsoft.EntityFrameworkCore;

namespace HappyTravel.Hiroshima.DirectContracts.Services.Availability
{
    public class RateAvailabilityService : IRateAvailabilityService
    {
        public RateAvailabilityService(DirectContractsDbContext dbContext,
            ICancellationPolicyService cancellationPolicyService,
            IPaymentDetailsService paymentDetailsService
            )
        {
            _dbContext = dbContext;
            _cancellationPolicyService = cancellationPolicyService;
            _paymentDetailsService = paymentDetailsService;
        }

        
        public async Task<List<RateOffer>> GetAvailableRates(IEnumerable<Room> rooms, DateTime checkInDate,
            DateTime checkOutDate, string languageCode)
        {
            var roomsDictionary = rooms.ToDictionary(r => r.Id);
            var roomIds = roomsDictionary.Keys;
            var rates = await GetRateDetails(roomIds.ToList(), checkInDate, checkOutDate, languageCode);
            var roomsPromotionalOffers =
                await GetPromotionalOffers(roomIds, checkInDate, checkOutDate, languageCode);
            
            var groupedPromotionalOffers = roomsPromotionalOffers.GroupBy(po => po.RoomId)
                .ToDictionary(g=> g.Key, g=>g.ToList());

            var cancellationPolicies = (await GetCancellationPolicies(roomIds, checkInDate)).ToDictionary(rcp =>
                rcp.RoomId);


            var availableRates = new List<RateOffer>();
            foreach (var roomRateGroup in rates.GroupBy(rateDetails => rateDetails.RoomRate.RoomId))
            {
                var roomRates = roomRateGroup.ToList();
                if (!roomRates.Any()) continue;

                var roomId = roomRateGroup.Key;
                var room = roomsDictionary[roomId];

                var roomPromotionalOffers = groupedPromotionalOffers[roomId];
                
                var paymentDetails = _paymentDetailsService.Create(checkInDate, checkOutDate, roomRates, roomPromotionalOffers);

                var roomCancellationPolicies = cancellationPolicies[roomId];

                var cancellationPolicyDetails =
                    _cancellationPolicyService.Get(roomCancellationPolicies, checkInDate, paymentDetails);

                availableRates.Add(new RateOffer
                {
                    Room = room, PaymentDetails = paymentDetails, CancellationPolicies = cancellationPolicyDetails
                });
            }

            return availableRates;
        }

        
        private async Task<List<RateDetails>> GetRateDetails(List<int> roomIds, DateTime checkInDate,
            DateTime checkOutDate, string languageCode)
        {
            checkInDate = checkInDate.Date;
            return await _dbContext.RoomRates
                .Join(_dbContext.Seasons, roomRate => roomRate.SeasonId, season => season.Id, (roomRate, season) => new {roomRate, season})
                .Where(roomRateAndSeason => roomIds.Contains(roomRateAndSeason.roomRate.RoomId) &&
                                            !(roomRateAndSeason.season.EndDate < checkInDate ||
                                              checkOutDate < roomRateAndSeason.season.StartDate))
                .Select(roomRateAndSeason => new RateDetails()
                {
                    RoomRate = new RoomRate
                    {
                        Id = roomRateAndSeason.roomRate.Id,
                        Details =
                            DirectContractsDbContext.GetLangFromJsonb(roomRateAndSeason.roomRate.Details,
                                languageCode),
                        Price = roomRateAndSeason.roomRate.Price,
                        BoardBasis = roomRateAndSeason.roomRate.BoardBasis,
                        CurrencyCode = roomRateAndSeason.roomRate.CurrencyCode,
                        SeasonId = roomRateAndSeason.roomRate.SeasonId,
                        MealPlan = roomRateAndSeason.roomRate.MealPlan,
                        RoomId = roomRateAndSeason.roomRate.RoomId
                    },
                    Season = roomRateAndSeason.season
                })
                .ToListAsync();
        }
        
        
        private async Task<List<RoomPromotionalOffer>> GetPromotionalOffers(IEnumerable<int> roomIds,
            DateTime checkInDate, DateTime checkOutDate, string languageCode)
        {
            checkInDate = checkInDate.Date;
            var dateNow = DateTime.UtcNow.Date;
            return await _dbContext.RoomPromotionalOffers
                .Where(offer => roomIds.Contains(offer.RoomId) &&
                                dateNow <= offer.BookByDate &&
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
        
        
        private async Task<List<RoomCancellationPolicy>> GetCancellationPolicies(IEnumerable<int> roomIds,
            DateTime checkInDate)
        {
            return await _dbContext.CancellationPolicies
                .Join(_dbContext.Seasons, roomCancellationPolicy
                        => roomCancellationPolicy.SeasonId, season => season.Id, (roomCancellationPolicy, season)
                        => new {roomCancellationPolicy, season}
                ).Where(roomCancellationPolicyAndSeason =>
                    roomIds.Contains(roomCancellationPolicyAndSeason.roomCancellationPolicy.RoomId) &&
                    roomCancellationPolicyAndSeason.season.StartDate.Date <= checkInDate &&
                    checkInDate <= roomCancellationPolicyAndSeason.season.EndDate.Date)
                .Select(roomCancellationPolicyAndSeason => roomCancellationPolicyAndSeason.roomCancellationPolicy)
                .ToListAsync();
        }
        
        
        private readonly IPaymentDetailsService _paymentDetailsService;
        private readonly ICancellationPolicyService _cancellationPolicyService;
        private readonly DirectContractsDbContext _dbContext;
    }
}