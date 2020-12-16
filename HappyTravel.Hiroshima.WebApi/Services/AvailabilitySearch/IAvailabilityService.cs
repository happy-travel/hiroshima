using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.EdoContracts.Accommodations;
using Microsoft.AspNetCore.Mvc;

namespace HappyTravel.Hiroshima.WebApi.Services.AvailabilitySearch
{
    public interface IAvailabilityService
    {
       Task<Result<Availability, ProblemDetails>> GetWideAvailability(AvailabilityRequest request, string languageCode);

       Task<Result<AccommodationAvailability, ProblemDetails>> GetAccommodationAvailability(string availabilityId, string accommodationId, string languageCode);

       Task<Result<RoomContractSetAvailability, ProblemDetails>> GetExactAvailability(string availabilityId, Guid roomContractSetId);
    }
}