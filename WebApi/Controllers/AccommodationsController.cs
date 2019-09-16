using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using HappyTravel.EdoContracts.Accommodations;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
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
