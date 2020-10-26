using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.EdoContracts.Accommodations;
using Microsoft.AspNetCore.Mvc;

namespace HappyTravel.Hiroshima.WebApi.Services
{
    public class AvailabilityService : IAvailabilityService
    {
       public AvailabilityService(
            HappyTravel.Hiroshima.DirectContracts.Services.Availability.IAvailabilityService availabilityService, IAvailabilityResponseFactory availabilityResponseFactory)
        {
            _availabilityService = availabilityService;
            _availabilityResponseFactory = availabilityResponseFactory;
        }


        public async Task<Result<Availability, ProblemDetails>> GetAvailabilityDetails(
            AvailabilityRequest availabilityRequest,
            string languageCode)
        {
            var availableRates = await _availabilityService.Get(availabilityRequest, languageCode);

           /* foreach (var availableRate in availableRates)
            {
                foreach (var availableRate1 in availableRates)
                {
                    
                }

                availableRate.
            }*/
            
            throw new NotImplementedException();
        }


        private readonly IAvailabilityResponseFactory _availabilityResponseFactory;
        private readonly HappyTravel.Hiroshima.DirectContracts.Services.Availability.IAvailabilityService _availabilityService;
    }
}