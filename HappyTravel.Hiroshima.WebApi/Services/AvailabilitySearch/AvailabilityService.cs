using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.EdoContracts.Accommodations;
using HappyTravel.Hiroshima.WebApi.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace HappyTravel.Hiroshima.WebApi.Services.AvailabilitySearch
{
    public class AvailabilityService : IAvailabilityService
    {
       public AvailabilityService(
            HappyTravel.Hiroshima.DirectContracts.Services.Availability.IAvailabilityService availabilityService, IAvailabilitySearchStore availabilitySearchStore, IAvailabilityResponseService availabilityResponseService)
        {
            _availabilityService = availabilityService;
            _availabilityResponseService = availabilityResponseService;
            _availabilitySearchStore = availabilitySearchStore;
        }


        public async Task<Result<Availability, ProblemDetails>> Get(AvailabilityRequest availabilityRequest, string languageCode)
        {
            var availableRates = await _availabilityService.Get(availabilityRequest, languageCode);
            
            var availabilityResult = _availabilityResponseService.Create(availabilityRequest, availableRates, languageCode);
            await _availabilitySearchStore.Add(availabilityResult);

            return availabilityResult;
        }


        public async Task<Result<AccommodationAvailability, ProblemDetails>> Get(string availabilityId, string accommodationId)
        {
            var availabilitySearchResult = await _availabilitySearchStore.Get(availabilityId);
            if (availabilitySearchResult.Equals(default))
                return Result.Failure<AccommodationAvailability, ProblemDetails>(ProblemDetailsBuilder.Build($"Failed to receive availability data with {nameof(availabilityId)} '{availabilityId}'"));

            return _availabilityResponseService.Create(accommodationId, availabilitySearchResult);
        }
        
        
        private readonly IAvailabilityResponseService _availabilityResponseService;
        private readonly HappyTravel.Hiroshima.DirectContracts.Services.Availability.IAvailabilityService _availabilityService;
        private readonly IAvailabilitySearchStore _availabilitySearchStore;
    }
}