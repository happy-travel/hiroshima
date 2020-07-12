using System.Net;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Common.Infrastructure;
using HappyTravel.Hiroshima.DirectManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace HappyTravel.Hiroshima.WebApi.Controllers.DirectManager
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}/management/contracts/accommodations")]
    [Produces("application/json")]
    public class AccommodationManagementController: ControllerBase
    {
        public AccommodationManagementController(IAccommodationManagementService accommodationManagementService)
        {
            _accommodationManagementService = accommodationManagementService;
        }
        
        
        /// <summary>
        /// Retrieves an accommodation by ID
        /// </summary>
        /// <param name="accommodationId">ID of the accommodation</param>
        /// <returns></returns>
        [HttpGet("{accommodationId}")]
        [ProducesResponseType(typeof(Hiroshima.DirectManager.Models.Responses.Accommodation), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAccommodation([FromRoute] int accommodationId)
        {
            var (_, isFailure, response, error) = await _accommodationManagementService.GetAccommodation(accommodationId);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response);
        }
        
        
        /// <summary>
        /// Creates a new accommodation
        /// </summary>
        /// <param name="accommodation">New accommodation data</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Hiroshima.DirectManager.Models.Responses.Accommodation), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddAccommodation([FromBody] Hiroshima.DirectManager.Models.Requests.Accommodation accommodation)
        {
            var (_, isFailure, response, error) = await _accommodationManagementService.AddAccommodation(accommodation);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response);
        }
        
        
        /// <summary>
        /// Updates an accommodation by ID
        /// </summary>
        /// <param name="accommodationId">ID of the accommodation</param>
        /// <param name="accommodation">New accommodation data</param>
        /// <returns></returns>
        [HttpPut("{accommodationId}")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateAccommodation([FromRoute] int accommodationId, [FromBody] Hiroshima.DirectManager.Models.Requests.Accommodation accommodation)
        {
            var (_, isFailure, error) = await _accommodationManagementService.Update(accommodationId, accommodation);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok();
        }
        
        
        /// <summary>
        /// Deletes an accommodation by ID
        /// </summary>
        /// <param name="accommodationId">ID of the accommodation</param>
        /// <returns></returns>
        [HttpDelete("{accommodationId}")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteAccommodation([FromRoute] int accommodationId)
        {
            var (_, isFailure, error) = await _accommodationManagementService.RemoveAccommodation(accommodationId);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok();
        }
        

        private readonly IAccommodationManagementService _accommodationManagementService;
    }
}