using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using HappyTravel.EdoContracts.Accommodations;
using Hiroshima.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hiroshima.WebApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}")]
    [Produces("application/json")]
    public class AccommodationsController : Controller
    {
        public AccommodationsController(IAvailabilityService availabilityService)
        {
            _availabilityService = availabilityService;
        }


        /// <summary>
        /// The method returns availability details
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("accommodations/availabilities")]
        [ProducesResponseType(typeof(AvailabilityDetails), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> SearchAvailabilities([FromBody] AvailabilityRequest request)
        {
            var result = await _availabilityService.SearchAvailability(request, CultureInfo.CurrentCulture.Name);
            if (result.IsFailure)
                return BadRequest(result.Error);
            return Ok(result);
        }


        private readonly IAvailabilityService _availabilityService;
    }
}
