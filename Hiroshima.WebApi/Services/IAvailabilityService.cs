using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.EdoContracts.Accommodations;
using Microsoft.AspNetCore.Mvc;

namespace Hiroshima.WebApi.Services
{
    public interface IAvailabilityService
    {
        Task<Result<AvailabilityDetails, ProblemDetails>> SearchAvailability(AvailabilityRequest request, string languageCode);
    }
}
