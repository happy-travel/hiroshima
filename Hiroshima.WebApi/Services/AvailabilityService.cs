using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.EdoContracts.Accommodations;
using Hiroshima.DirectContracts.Services.Availability;
using Hiroshima.WebApi.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Hiroshima.WebApi.Services
{
    public class AvailabilityService : IAvailabilityService
    {
        public AvailabilityService(
            IDcAvailability dcAvailability,
            IDcRequestConverter requestConverter,
            IDcResponseConverter responseConverter)
        {
            _dcAvailability = dcAvailability;
            _requestConverter = requestConverter;
            _responseConverter = responseConverter;
        }
        public async Task<Result<AvailabilityDetails, ProblemDetails>> SearchAvailability(
            AvailabilityRequest availabilityRequest, 
            string languageCode)
        {
            var dcRequest = _requestConverter.CreateAvailabilityRequest(availabilityRequest);
            if (dcRequest.IsFailure)
                return ProblemDetailsBuilder.Fail<AvailabilityDetails>(dcRequest.Error);

            var result = await _dcAvailability.SearchAvailableAgreements(dcRequest.Value);
            
            var dcResponse = _responseConverter.CreateAvailabilityDetails(result, languageCode);
            
            if (dcResponse.IsFailure)
                return ProblemDetailsBuilder.Fail<AvailabilityDetails>(dcResponse.Error);

            return Result.Ok<AvailabilityDetails, ProblemDetails>(dcResponse.Value);
        }

        private readonly IDcRequestConverter _requestConverter;
        private readonly IDcResponseConverter _responseConverter;
        private readonly IDcAvailability _dcAvailability;
    }
}
