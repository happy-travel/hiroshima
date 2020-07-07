using System.Net;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.DirectManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace HappyTravel.DirectManager.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}/management/contracts")]
    [Produces("application/json")]
    public class AccommodationsController: Controller
    {
        public AccommodationsController(IAccommodationManagementService accommodationManagement)
        {
            _accommodationManagement = accommodationManagement;
        }
        
        
        [HttpGet("/accommodations/{accommodationId}")]
        [ProducesResponseType(typeof(Models.Responses.Accommodation), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAccommodation([FromRoute] string contractId, [FromRoute] int accommodationId)
        {
            var (_, isFailure, response, error) = await _accommodationManagement.GetAccommodation(accommodationId);
            if (isFailure)
                return BadRequest(error);

            return Ok(response);
        }
        
        
        [HttpPost("{contractId}/accommodations")]
        [ProducesResponseType(typeof(Models.Responses.Accommodation), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddAccommodation([FromRoute] string contractId, [FromBody] Models.Requests.Accommodation accommodation)
        {
            var (_, isFailure, response, error) = await _accommodationManagement.AddAccommodation(accommodation);
            if (isFailure)
                return BadRequest(error);

            return Ok(response);
        }
        
        
        [HttpPut("{contractId}/accommodations/{accommodationId}")]
        [ProducesResponseType(typeof(Models.Responses.Accommodation), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateAccommodation([FromRoute] string accommodationId, [FromBody] Models.Requests.Accommodation accommodation)
        {
            var (_, isFailure, response, error) = await _accommodationManagement.UpdateAccommodation(accommodationId, accommodation);
            if (isFailure)
                return BadRequest(error);

            return Ok(response);
        }
        
        
        [HttpDelete("{contractId}/accommodations/{accommodationId}")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteAccommodation([FromRoute] int contractId, [FromRoute] string accommodationId)
        {
            var (_, isFailure,  error) = await _accommodationManagement.DeleteAccommodation(accommodationId);
            if (isFailure)
                return BadRequest(error);

            return Ok();
        }
        

        private readonly IAccommodationManagementService _accommodationManagement;
    }
}