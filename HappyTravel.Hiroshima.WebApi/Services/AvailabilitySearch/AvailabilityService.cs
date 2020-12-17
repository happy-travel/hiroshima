using System;
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
            HappyTravel.Hiroshima.DirectContracts.Services.Availability.IAvailabilityService availabilityService, IAvailabilitySearchStorage availabilitySearchStorage, IAvailabilityResponseService availabilityResponseService, IAvailabilityIdMatchingService availabilityIdMatchingService)
        {
            _availabilityService = availabilityService;
            _availabilityResponseService = availabilityResponseService;
            _availabilitySearchStorage = availabilitySearchStorage;
            _availabilityIdMatchingService = availabilityIdMatchingService;
        }


        public async Task<Result<Availability, ProblemDetails>> GetWideAvailability(AvailabilityRequest availabilityRequest, string languageCode)
        {
            var availability = await _availabilityService.Get(availabilityRequest, languageCode);
            if (!availability.AvailableRates.Any())
                return _availabilityResponseService.CreateEmptyAvailability(availabilityRequest);
            
            var availabilityResult = _availabilityResponseService.Create(availabilityRequest, availability, languageCode);
            await _availabilitySearchStorage.AddAvailabilityRequest(availabilityResult.AvailabilityId, availabilityRequest);
            
            return availabilityResult;
        }


        public async Task<Result<AccommodationAvailability, ProblemDetails>> GetAccommodationAvailability(string availabilityId, string accommodationId, string languageCode)
        {
            var availabilityRequest = await _availabilitySearchStorage.GetAvailabilityRequest(availabilityId);
            
            if (availabilityRequest.Equals(default))
                return Result.Failure<AccommodationAvailability, ProblemDetails>(ProblemDetailsBuilder.Build($"Failed to retrieve availability data with {nameof(availabilityId)} '{availabilityId}'"));
            
            if (!int.TryParse(accommodationId,  out var parsedAccommodationId))
                return Result.Failure<AccommodationAvailability, ProblemDetails>(ProblemDetailsBuilder.Build($"Failed to retrieve accommodation data with {nameof(accommodationId)} '{accommodationId}'"));
                
            var availability = await _availabilityService.Get(availabilityRequest, parsedAccommodationId, languageCode);

            if (!availability.AvailableRates.Any())
                return _availabilityResponseService.CreateEmptyAccommodationAvailability(availabilityRequest);

            await _availabilityIdMatchingService.SetAccommodationAvailabilityId(availabilityId, availability.Id);
            availability.Id = availabilityId;
            
            var availabilityResult = _availabilityResponseService.CreateAccommodationAvailability(availabilityRequest, availability, languageCode);

            await _availabilitySearchStorage.AddAccommodationAvailability(availabilityResult);

            return availabilityResult;
        }

 
        public async Task<Result<RoomContractSetAvailability, ProblemDetails>> GetExactAvailability(string availabilityId, Guid roomContractSetId)
        {
            var accommodationAvailability = await _availabilitySearchStorage.GetAccommodationAvailability(availabilityId);
            if (string.IsNullOrEmpty(accommodationAvailability.AvailabilityId))
                return Result.Failure<RoomContractSetAvailability, ProblemDetails>(ProblemDetailsBuilder.Build($"Failed to retrieve availability data with {nameof(availabilityId)} '{availabilityId}'"));
            
            var availabilityResult = _availabilityResponseService.Create(accommodationAvailability, roomContractSetId);

            return availabilityResult;
        }
        
        
        private readonly IAvailabilityResponseService _availabilityResponseService;
        private readonly HappyTravel.Hiroshima.DirectContracts.Services.Availability.IAvailabilityService _availabilityService;
        private readonly IAvailabilitySearchStorage _availabilitySearchStorage;
        private readonly IAvailabilityIdMatchingService _availabilityIdMatchingService;
    }
}