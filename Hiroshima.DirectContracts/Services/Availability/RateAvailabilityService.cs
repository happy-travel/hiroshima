using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hiroshima.DbData.Models.Room;
using Hiroshima.DirectContracts.Models;

namespace Hiroshima.DirectContracts.Services.Availability
{
    public class RateAvailabilityService : IRateAvailabilityService
    {
        public RateAvailabilityService(IAvailabilityRepository availabilityRepository,
            ICancellationPoliciesService cancellationPoliciesService,
            IPaymentDetailsService paymentDetailsService
            )
        {
            _availabilityRepository = availabilityRepository;
            _cancellationPoliciesService = cancellationPoliciesService;
            _paymentDetailsService = paymentDetailsService;
        }

        
        public async Task<List<RateOffer>> GetAvailableRates(IEnumerable<Room> rooms, DateTime checkInDate,
            DateTime checkOutDate, string languageCode)
        {
            var roomsDictionary = rooms.ToDictionary(r => r.Id);
            var roomIds = roomsDictionary.Keys;
            var rates = await _availabilityRepository.GetRates(roomIds, checkInDate, checkOutDate, languageCode);
            var promotionalOffers =
                (await _availabilityRepository.GetPromotionalOffers(roomIds, checkInDate, languageCode))
                .ToDictionary(rpo => rpo.RoomId);
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

                var promotionalOffer = promotionalOffers[roomId];
                
                var paymentDetails = _paymentDetailsService.Create(checkInDate, checkOutDate, roomRates, promotionalOffer);

                var roomCancellationPolicies = cancellationPolicies[roomId];

                var cancellationPolicyDetails =
                    _cancellationPoliciesService.Get(roomCancellationPolicies, checkInDate, paymentDetails);

                availableRates.Add(new RateOffer
                {
                    Room = room, PaymentDetails = paymentDetails, CancellationPolicies = cancellationPolicyDetails
                });
            }

            return availableRates;
        }

        
        private readonly IPaymentDetailsService _paymentDetailsService;
        private readonly ICancellationPoliciesService _cancellationPoliciesService;
        private readonly IAvailabilityRepository _availabilityRepository;
    }
}