using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HappyTravel.Hiroshima.Data.Models.Rooms;
using HappyTravel.Hiroshima.DirectContracts.Models;

namespace HappyTravel.Hiroshima.DirectContracts.Services.Availability
{
    public class RateAvailabilityService : IRateAvailabilityService
    {
        public RateAvailabilityService(IAvailabilityRepository availabilityRepository,
            ICancellationPolicyService cancellationPolicyService,
            IPaymentDetailsService paymentDetailsService
            )
        {
            _availabilityRepository = availabilityRepository;
            _cancellationPolicyService = cancellationPolicyService;
            _paymentDetailsService = paymentDetailsService;
        }

        
        public async Task<List<RateOffer>> GetAvailableRates(IEnumerable<Room> rooms, DateTime checkInDate,
            DateTime checkOutDate, string languageCode)
        {
            var roomsDictionary = rooms.ToDictionary(r => r.Id);
            var roomIds = roomsDictionary.Keys;
            var rates = await _availabilityRepository.GetRates(roomIds, checkInDate, checkOutDate, languageCode);
            var roomsPromotionalOffers =
                await _availabilityRepository.GetPromotionalOffers(roomIds, checkInDate, checkOutDate, languageCode);
            
            var groupedPromotionalOffers = roomsPromotionalOffers.GroupBy(po => po.RoomId)
                .ToDictionary(g=>g.Key, g=>g.ToList());
            
            var cancellationPolicies =
                (await _availabilityRepository.GetCancellationPolicies(roomIds, checkInDate)).ToDictionary(rcp =>
                    rcp.RoomId);

            var availableRates = new List<RateOffer>();
            foreach (var roomRateGroup in rates.GroupBy(rr => rr.RoomId))
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

        
        private readonly IPaymentDetailsService _paymentDetailsService;
        private readonly ICancellationPolicyService _cancellationPolicyService;
        private readonly IAvailabilityRepository _availabilityRepository;
    }
}