using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.EdoContracts.Accommodations;
using HappyTravel.Hiroshima.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace HappyTravel.Hiroshima.WebApi.Controllers.Connector
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}/accommodations")]
    [Produces("application/json")]
    public class AccommodationsController : ControllerBase
    {
        public AccommodationsController(IAvailabilityService availabilityService)
        {
            _availabilityService = availabilityService;
        }


        /// <summary>
        /// Returns available accommodations with room contracts
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("availabilities")]
        [ProducesResponseType(typeof(AvailabilityDetails), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAvailability([FromBody] AvailabilityRequest request)
        {
            var (_, isFailure, value, error) = await _availabilityService.GetAvailabilityDetails(request, LanguageCode);
            if (isFailure)
                return BadRequest(error);

            return Ok(value);
        }

        
        private string LanguageCode => CultureInfo.CurrentCulture.Name;
        private readonly IAvailabilityService _availabilityService;
    }
}