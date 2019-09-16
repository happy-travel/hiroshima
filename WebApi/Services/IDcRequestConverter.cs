using CSharpFunctionalExtensions;
using HappyTravel.EdoContracts.Accommodations;
using Hiroshima.DirectContracts.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Services
{
    public interface IDcRequestConverter
    {
        Result<DcAvailabilityRequest> CreateAvailabilityRequest(AvailabilityRequest request);
    }
}
