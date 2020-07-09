using System.Net;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Common.Infrastructure;
using HappyTravel.Hiroshima.DirectManager.Models.Requests;
using HappyTravel.Hiroshima.DirectManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace HappyTravel.Hiroshima.DirectManager.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}/management/contracts/accommodations")]
    [Produces("application/json")]
    public class AccommodationsController: Controller
    {
        public AccommodationsController(IAccommodationManagementService accommodationManagement)
        {
            _accommodationManagement = accommodationManagement;
        }
        
        
        /// <summary>
        /// Retrieves an accommodation by ID
        /// </summary>
        /// <param name="accommodationId"></param>
        /// <returns></returns>
        [HttpGet("${accommodationId}")]
        [ProducesResponseType(typeof(Models.Responses.Accommodation), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAccommodation([FromRoute] int accommodationId)
        {
            var (_, isFailure, response, error) = await _accommodationManagement.GetAccommodation(accommodationId);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response);
        }
        
        
        /// <summary>
        /// Creates a new accommodation
        /// </summary>
        /// <param name="accommodation"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Models.Responses.Accommodation), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddAccommodation([FromBody] Accommodation accommodation)
        {
            var (_, isFailure, response, error) = await _accommodationManagement.AddAccommodation(accommodation);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response);
        }
        
        
        /// <summary>
        /// Updates an accommodation by ID
        /// </summary>
        /// <param name="accommodationId"></param>
        /// <param name="accommodation"></param>
        /// <returns></returns>
        [HttpPut("{accommodationId}")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateAccommodation([FromRoute] string accommodationId, [FromBody] Accommodation accommodation)
        {
            var (_, isFailure, error) = await _accommodationManagement.UpdateAccommodation(accommodationId, accommodation);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok();
        }
        
        
        /// <summary>
        /// Deletes an accommodation by ID
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="accommodationId"></param>
        /// <returns></returns>
        [HttpDelete("{accommodationId}")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteAccommodation([FromRoute] int contractId, [FromRoute] string accommodationId)
        {
            var (_, isFailure, error) = await _accommodationManagement.DeleteAccommodation(accommodationId);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok();
        }
        

        private readonly IAccommodationManagementService _accommodationManagement;
    }
}