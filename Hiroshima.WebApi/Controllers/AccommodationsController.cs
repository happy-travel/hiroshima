using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.EdoContracts.Accommodations;
using Hiroshima.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hiroshima.WebApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}/accommodations")]
    [Produces("application/json")]
    public class AccommodationsController : Controller
    {
        public AccommodationsController(IAvailabilityService availabilityService)
        {
            _availabilityService = availabilityService;
        }


        [HttpPost("availabilities")]
        [ProducesResponseType(typeof(AvailabilityDetails), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAvailability([FromBody] AvailabilityRequest request)
        {
            //TODO change LanguageCode !!!
            var (_, isFailure, value, error) = await _availabilityService.GetAvailabilityDetails(request, LanguageCode);
            if (isFailure)
                return BadRequest(error);

            return Ok(value);
        }

        
        private string LanguageCode => CultureInfo.CurrentCulture.Name;
        private readonly IAvailabilityService _availabilityService;
    }
}