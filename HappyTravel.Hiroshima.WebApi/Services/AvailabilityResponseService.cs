using System;
using System.Collections.Generic;
using HappyTravel.Hiroshima.Common.Models.Accommodations;
using HappyTravel.Hiroshima.DirectContracts.Models;

namespace HappyTravel.Hiroshima.WebApi.Services
{
    public class AvailabilityResponseService : IAvailabilityResponseService
    {
        public AvailabilityResponseService(IAccommodationResponseService accommodationResponseService, IRateResponseService rateResponseService)
        {
            _accommodationResponseService = accommodationResponseService;
            _rateResponseService = rateResponseService;
        }


        public EdoContracts.Accommodations.Availability Create(EdoContracts.Accommodations.AvailabilityRequest availabilityRequest, Dictionary<Accommodation, List<AvailableRates>> accommodationAvailableRatesStore, string languageCode)
        {
            var availabilityId = CreateAvailabilityId();
            var numberOfNights = GetNumberOfNights(availabilityRequest.CheckInDate, availabilityRequest.CheckOutDate);
            var numberOfProcessedAccommodations = accommodationAvailableRatesStore.Count;
            var slimAccommodationAvailabilities = CreateSlimAccommodationAvailabilities(accommodationAvailableRatesStore, languageCode); 
            
            return new EdoContracts.Accommodations.Availability(availabilityId, numberOfNights, availabilityRequest.CheckInDate.Date, availabilityRequest.CheckOutDate.Date, slimAccommodationAvailabilities, numberOfProcessedAccommodations);
        }


        private List<EdoContracts.Accommodations.Internals.SlimAccommodationAvailability> CreateSlimAccommodationAvailabilities(Dictionary<Accommodation, List<AvailableRates>> accommodationAvailableRatesStore, string languageCode)
        {
            var slimAccommodationAvailabilities = new List<EdoContracts.Accommodations.Internals.SlimAccommodationAvailability>();
                
            foreach (var accommodationAvailableRate in accommodationAvailableRatesStore)
            {
                var slimAccommodationAvailability = CreateSlimAccommodationAvailability(accommodationAvailableRate.Key, accommodationAvailableRate.Value); 
                slimAccommodationAvailabilities.Add(slimAccommodationAvailability);
            }

            return slimAccommodationAvailabilities;


            EdoContracts.Accommodations.Internals.SlimAccommodationAvailability CreateSlimAccommodationAvailability(Accommodation accommodation, List<AvailableRates> availableRates)
            {
                var slimAccommodation = _accommodationResponseService.Create(accommodation, languageCode);
                var roomContractSets = _rateResponseService.Create(availableRates);
                var availabilityId = CreateAvailabilityId();
                
                return new EdoContracts.Accommodations.Internals.SlimAccommodationAvailability(slimAccommodation, roomContractSets, availabilityId);
                throw new NotImplementedException();
            }
        }

        private string CreateAvailabilityId() => Guid.NewGuid().ToString("N");


        private int GetNumberOfNights(DateTime checkInDate, DateTime checkOutDate) => (checkOutDate.Date - checkInDate.Date).Days;

        
        private readonly IAccommodationResponseService _accommodationResponseService;
        private readonly IRateResponseService _rateResponseService;
        
    }
}