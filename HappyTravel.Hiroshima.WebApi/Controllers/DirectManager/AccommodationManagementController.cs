using System.Collections.Generic;
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
            var (_, isFailure, response, error) = await _accommodationManagementService.Get(accommodationId);
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
            var (_, isFailure, response, error) = await _accommodationManagementService.Add(accommodation);
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

            return NoContent();
        }
        
        
        /// <summary>
        /// Removes an accommodation by ID
        /// </summary>
        /// <param name="accommodationId">ID of the accommodation</param>
        /// <returns></returns>
        [HttpDelete("{accommodationId}")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RemoveAccommodation([FromRoute] int accommodationId)
        {
            var (_, isFailure, error) = await _accommodationManagementService.Remove(accommodationId);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return NoContent();
        }

        
        /// <summary>
        /// Retrieves accommodation rooms
        /// </summary>
        /// <param name="accommodationId"></param>
        /// <returns></returns>
        [HttpGet("{accommodationId}/rooms")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetRooms([FromRoute] int accommodationId)
        { 
            var (_, isFailure, response, error) = await _accommodationManagementService.GetRooms(accommodationId);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response);
        }
        
        
        /// <summary>
        /// Adds rooms to the accommodation
        /// </summary>
        /// <param name="accommodationId"></param>
        /// <param name="rooms"></param>
        /// <returns></returns>
        [HttpPost("{accommodationId}/rooms")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        public async Task<IActionResult> AddRooms([FromRoute] int accommodationId, [FromBody] List<Hiroshima.DirectManager.Models.Requests.Room> rooms)
        { 
            var (_, isFailure, response, error) = await _accommodationManagementService.AddRooms(accommodationId, rooms);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response);
        }
        
        /// <summary>
        /// Removes a accommodation room of by ID
        /// </summary>
        /// <param name="accommodationId"></param>
        /// <param name="roomId"></param>
        /// <returns></returns>
        [HttpDelete("{accommodationId}/rooms/{roomId}")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteRoom([FromRoute] int accommodationId, [FromRoute] int roomId)
        { 
            var (_, isFailure, error) = await _accommodationManagementService.RemoveRooms(accommodationId, new List<int>{roomId});
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return NoContent();
        }
        
        private readonly IAccommodationManagementService _accommodationManagementService;
    }
}