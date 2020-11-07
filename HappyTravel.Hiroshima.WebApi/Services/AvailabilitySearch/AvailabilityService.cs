using System.Linq;
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
            var accommodationsWithAvailableRates = await _availabilityService.Get(availabilityRequest, languageCode);
            if (!accommodationsWithAvailableRates.Any())
                return _availabilityResponseService.CreateEmptyAvailability(availabilityRequest);
            
            var availabilityResult = _availabilityResponseService.Create(availabilityRequest, accommodationsWithAvailableRates, languageCode);
            await _availabilitySearchStore.AddAvailabilityRequest(availabilityResult.AvailabilityId, availabilityRequest);

            return availabilityResult;
        }


        public async Task<Result<AccommodationAvailability, ProblemDetails>> Get(string availabilityId, string accommodationId, string languageCode)
        {
            var availabilityRequest = await _availabilitySearchStore.GetAvailabilityRequest(availabilityId);
            
            if (availabilityRequest.Equals(default))
                return Result.Failure<AccommodationAvailability, ProblemDetails>(ProblemDetailsBuilder.Build($"Failed to retrieve availability data with {nameof(availabilityId)} '{availabilityId}'"));
            
            if (!int.TryParse(accommodationId,  out var parsedAccommodationId))
                return Result.Failure<AccommodationAvailability, ProblemDetails>(ProblemDetailsBuilder.Build($"Failed to retrieve accommodation data with {nameof(accommodationId)} '{accommodationId}'"));
                
            var accommodationsWithAvailableRates = await _availabilityService.Get(availabilityRequest, parsedAccommodationId, languageCode);

            if (!accommodationsWithAvailableRates.Any())
                return _availabilityResponseService.CreateEmptyAccommodationAvailability(availabilityRequest);

            var accommodationsWithAvailableRate = accommodationsWithAvailableRates.First();
            
            var availabilityResult = _availabilityResponseService.Create(availabilityRequest, accommodationsWithAvailableRate, languageCode);

            return availabilityResult;
        }
        
        
        private readonly IAvailabilityResponseService _availabilityResponseService;
        private readonly HappyTravel.Hiroshima.DirectContracts.Services.Availability.IAvailabilityService _availabilityService;
        private readonly IAvailabilitySearchStore _availabilitySearchStore;
    }
}