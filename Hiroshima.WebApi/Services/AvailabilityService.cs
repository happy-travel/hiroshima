using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.EdoContracts.Accommodations;
using Hiroshima.Common.Models.Enums;
using Hiroshima.Common.Utils.Languages;
using Hiroshima.DirectContracts.Services;
using Hiroshima.WebApi.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Hiroshima.WebApi.Services
{
    public class AvailabilityService : IAvailabilityService
    {
       public AvailabilityService(
            IDirectContractsAvailabilityService directContractsAvailabilityService)
        {
            _directContractsAvailabilityService = directContractsAvailabilityService;
        }


        public async Task<Result<AvailabilityDetails, ProblemDetails>> GetAvailabilityDetails(
            AvailabilityRequest availabilityRequest,
            string languageCode)
        {
            var language = LanguageUtils.GetLanguage(languageCode);
            if (language == Languages.Unknown)
                return ProblemDetailsBuilder.Fail<AvailabilityDetails>($"{nameof(languageCode)} is unknown");

            var result = await _directContractsAvailabilityService.GetAvailable(availabilityRequest, language);

            return Result.Ok<AvailabilityDetails, ProblemDetails>(result);
        }


        private readonly IDirectContractsAvailabilityService _directContractsAvailabilityService;
    }
}