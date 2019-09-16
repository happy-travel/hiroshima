using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.EdoContracts.Accommodations;
using Hiroshima.DbData;
using Hiroshima.DirectContracts;
using Microsoft.AspNetCore.Mvc;
using NodaTime;
using WebApi.Infrastructure;

namespace WebApi.Services
{
    public class AvailabilityService : IAvailabilityService
    {
        public AvailabilityService(
            IDirectContracts directContracts, 
            IDcRequestConverter requestConverter,
            IDcResponseConverter responseConverter)
        {
            _directContracts = directContracts;
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

            var result = await _directContracts
                .AvailabilitySearch
                .SearchAvailableAgreements(dcRequest.Value);

            var dcResponse = _responseConverter.CreateAvailabilityDetails(result,languageCode);
            
            if (dcResponse.IsFailure)
                return ProblemDetailsBuilder.Fail<AvailabilityDetails>(dcResponse.Error);

            return Result.Success<AvailabilityDetails, ProblemDetails>(dcResponse.Value);
            
        }

        private readonly IDcRequestConverter _requestConverter;
        private readonly IDcResponseConverter _responseConverter;
        private readonly IDirectContracts _directContracts;
    }
}
