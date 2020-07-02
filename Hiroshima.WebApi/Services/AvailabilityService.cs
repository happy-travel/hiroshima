using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.EdoContracts.Accommodations;
using Hiroshima.DirectContracts.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hiroshima.WebApi.Services
{
    public class AvailabilityService : IAvailabilityService
    {
       public AvailabilityService(
            DirectContracts.Services.Availability.IAvailabilityService availabilityService)
        {
            _availabilityService = availabilityService;
        }


        public async Task<Result<AvailabilityDetails, ProblemDetails>> GetAvailabilityDetails(
            AvailabilityRequest availabilityRequest,
            string languageCode)
        {
            var result = await _availabilityService.Get(availabilityRequest, languageCode);
            
            
            throw new NotImplementedException();
        }


        private readonly DirectContracts.Services.Availability.IAvailabilityService _availabilityService;
    }
}