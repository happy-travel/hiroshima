using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;

using Microsoft.AspNetCore.Mvc;

namespace HappyTravel.Hiroshima.WebApi.Services
{
    public class AvailabilityResponseFactory : IAvailabilityResponseFactory
    {
        public AvailabilityResponseFactory()
        { }


        public Task<Result<EdoContracts.Accommodations.Availability, ProblemDetails>> Create(EdoContracts.Accommodations.AvailabilityRequest availabilityRequest, string languageCode)
        {
            throw new NotImplementedException();
        }
    }
}