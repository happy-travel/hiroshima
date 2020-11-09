using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.EdoContracts.Accommodations;
using Microsoft.AspNetCore.Mvc;

namespace HappyTravel.Hiroshima.WebApi.Services.AvailabilitySearch
{
    public interface IAvailabilityService
    {
       Task<Result<Availability, ProblemDetails>> Get(AvailabilityRequest request, string languageCode);

       Task<Result<AccommodationAvailability, ProblemDetails>> Get(string availabilityId, string accommodationId, string languageCode);

       Task<Result<RoomContractSetAvailability, ProblemDetails>> Get(string availabilityId, Guid roomContractSetId);
    }
}