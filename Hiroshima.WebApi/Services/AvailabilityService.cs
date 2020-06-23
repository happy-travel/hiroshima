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
            IDcAvailabilityService dcAvailabilityService)
        {
            _dcAvailabilityService = dcAvailabilityService;
        }


        public async Task<Result<AvailabilityDetails, ProblemDetails>> GetAvailabilityDetails(
            AvailabilityRequest availabilityRequest,
            string languageCode)
        {
            var result = await _dcAvailabilityService.GetAvailable(availabilityRequest, languageCode);
            
            
            throw new NotImplementedException();
        }


        private readonly IDcAvailabilityService _dcAvailabilityService;
    }
}