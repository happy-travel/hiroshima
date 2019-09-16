using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.EdoContracts.Accommodations;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Services
{
    public interface IAvailabilityService
    {
        Task<Result<AvailabilityDetails, ProblemDetails>> SearchAvailability(AvailabilityRequest request, string languageCode);
    }
}
