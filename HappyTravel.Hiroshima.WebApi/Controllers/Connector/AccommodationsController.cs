using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.EdoContracts.Accommodations;
using HappyTravel.Hiroshima.WebApi.Infrastructure.Attributes;
using HappyTravel.Hiroshima.WebApi.Services;
using HappyTravel.Hiroshima.WebApi.Services.AvailabilitySearch;
using Microsoft.AspNetCore.Mvc;

namespace HappyTravel.Hiroshima.WebApi.Controllers.Connector
{
    [ApiController]
    [IgnoreLocalizationConvention]
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}/accommodations")]
    [Produces("application/json")]
    public class AccommodationsController : BaseController
    {
        public AccommodationsController(IAvailabilityService availabilityService, IAccommodationDataService accommodationDataService)
        {
            _availabilityService = availabilityService;
            _accommodationDataService = accommodationDataService;
        }


        /// <summary>
        /// Retrieves available accommodations with room contract sets.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("availabilities")]
        [ProducesResponseType(typeof(Availability), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Get([FromBody] AvailabilityRequest request)
        {
            var (_, isFailure, value, error) = await _availabilityService.GetWideAvailability(request, LanguageCode);
            if (isFailure)
                return BadRequest(error);

            return Ok(value);
        }

        
        /// <summary>
        /// Retrieves the accommodation with available room contract sets. 
        /// </summary>
        /// <param name="accommodationId"></param>
        /// <param name="availabilityId"></param>
        /// <returns></returns>
        [HttpPost("{accommodationId}/availabilities/{availabilityId}")]
        [ProducesResponseType(typeof(AccommodationAvailability), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Get([FromRoute] string accommodationId, [FromRoute] string availabilityId)
        {
            var (_, isFailure, result, error) = await _availabilityService.GetAccommodationAvailability(availabilityId, accommodationId, LanguageCode);
            if (isFailure)
                return BadRequest(error);

            return Ok(result);
        }
        
        
        /// <summary>
        /// Retrieves the room contract set.
        /// </summary>
        /// <param name="availabilityId"></param>
        /// <param name="roomContractSetId"></param>
        /// <returns></returns>
        [HttpPost("availabilities/{availabilityId}/room-contract-sets/{roomContractSetId}")]
        [ProducesResponseType(typeof(RoomContractSetAvailability), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Get([FromRoute] string availabilityId, [FromRoute] Guid roomContractSetId)
        {
            var (_, isFailure, result, error) = await _availabilityService.GetExactAvailability(availabilityId, roomContractSetId);
            if (isFailure)
                return BadRequest(error);

            return Ok(result);
        }
        
        
        /// <summary>
        /// Retrieves an accommodation details
        /// </summary>
        /// <param name="accommodationId">Accommodation id</param>
        /// <returns></returns>
        [HttpGet("{accommodationId}")]
        [ProducesResponseType(typeof(Accommodation), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAccommodationDetails([Required] [FromRoute] string accommodationId)
        {
            var (_, isFailure, accommodationDetails, problemDetails) = await _accommodationDataService.GetAccommodationDetails(Convert.ToInt32(accommodationId), LanguageCode);
            if (isFailure)
                return BadRequest(problemDetails);

            return Ok(accommodationDetails);
        }
        
        
        private readonly IAvailabilityService _availabilityService;
        private readonly IAccommodationDataService _accommodationDataService;
    }
}