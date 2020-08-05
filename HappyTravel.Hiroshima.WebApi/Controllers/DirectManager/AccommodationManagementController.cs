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
        [ProducesResponseType(typeof(Hiroshima.DirectManager.Models.Responses.AccommodationResponse), (int) HttpStatusCode.OK)]
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
        /// <param name="accommodationRequest">New accommodation data</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Hiroshima.DirectManager.Models.Responses.AccommodationResponse), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddAccommodation([FromBody] Hiroshima.DirectManager.Models.Requests.AccommodationRequest accommodationRequest)
        {
            var (_, isFailure, response, error) = await _accommodationManagementService.Add(accommodationRequest);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response);
        }
        
        
        /// <summary>
        /// Updates an accommodation by ID
        /// </summary>
        /// <param name="accommodationId">ID of the accommodation</param>
        /// <param name="accommodationRequest">New accommodation data</param>
        /// <returns></returns>
        [HttpPut("{accommodationId}")]
        [ProducesResponseType(typeof(Hiroshima.DirectManager.Models.Responses.AccommodationResponse), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateAccommodation([FromRoute] int accommodationId, [FromBody] Hiroshima.DirectManager.Models.Requests.AccommodationRequest accommodationRequest)
        {
            var (_, isFailure, response, error) = await _accommodationManagementService.Update(accommodationId, accommodationRequest);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response);
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
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
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
        [ProducesResponseType(typeof(List<Hiroshima.DirectManager.Models.Responses.RoomResponse>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddRooms([FromRoute] int accommodationId, [FromBody] List<Hiroshima.DirectManager.Models.Requests.RoomRequest> rooms)
        { 
            var (_, isFailure, response, error) = await _accommodationManagementService.AddRooms(accommodationId, rooms);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response);
        }
        
        
        /// <summary>
        /// Removes an accommodation room by ID
        /// </summary>
        /// <param name="accommodationId"></param>
        /// <param name="roomId"></param>
        /// <returns></returns>
        [HttpDelete("{accommodationId}/rooms/{roomId}")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RemoveRoom([FromRoute] int accommodationId, [FromRoute] int roomId)
        { 
            var (_, isFailure, error) = await _accommodationManagementService.RemoveRooms(accommodationId, new List<int>{roomId});
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return NoContent();
        }
        
        
        /// <summary>
        /// Removes an accommodation rooms by ID
        /// </summary>
        /// <param name="accommodationId">Accommodation id</param>
        /// <param name="ids">Room ids</param>
        /// <returns></returns>
        [HttpDelete("{accommodationId}/rooms")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RemoveRooms([FromRoute] int accommodationId, [FromBody] List<int> ids)
        { 
            var (_, isFailure, error) = await _accommodationManagementService.RemoveRooms(accommodationId, ids);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return NoContent();
        }
        
        
        private readonly IAccommodationManagementService _accommodationManagementService;
    }
}