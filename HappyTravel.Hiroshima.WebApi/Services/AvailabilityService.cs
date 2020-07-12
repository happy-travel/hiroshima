using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.EdoContracts.Accommodations;
using Microsoft.AspNetCore.Mvc;
using NetTopologySuite.Geometries;

namespace HappyTravel.Hiroshima.WebApi.Services
{
    public class AvailabilityService : IAvailabilityService
    {
       public AvailabilityService(
            HappyTravel.Hiroshima.DirectContracts.Services.Availability.IAvailabilityService availabilityService, GeometryFactory geometryFactory)
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


        private readonly HappyTravel.Hiroshima.DirectContracts.Services.Availability.IAvailabilityService _availabilityService;
    }
}