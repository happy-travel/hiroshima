using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.EdoContracts.Accommodations;
using Microsoft.AspNetCore.Mvc;

namespace HappyTravel.Hiroshima.WebApi.Services
{
    public class AvailabilityService : IAvailabilityService
    {
       public AvailabilityService(
            HappyTravel.Hiroshima.DirectContracts.Services.Availability.IAvailabilityService availabilityService, IAvailabilityResponseService availabilityResponseService)
        {
            _availabilityService = availabilityService;
            _availabilityResponseService = availabilityResponseService;
        }


        public async Task<Result<Availability, ProblemDetails>> GetAvailabilityDetails(
            AvailabilityRequest availabilityRequest,
            string languageCode)
        {
            var availableRates = await _availabilityService.Get(availabilityRequest, languageCode);
            
            return _availabilityResponseService.Create(availabilityRequest, availableRates, languageCode);
        }


        private readonly IAvailabilityResponseService _availabilityResponseService;
        private readonly HappyTravel.Hiroshima.DirectContracts.Services.Availability.IAvailabilityService _availabilityService;
    }
}