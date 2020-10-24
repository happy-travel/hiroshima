using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.DirectManager.Services;
using HappyTravel.Hiroshima.WebApi.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace HappyTravel.Hiroshima.WebApi.Controllers.DirectManager
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}/management/contracts")]
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
        [HttpGet("accommodations/{accommodationId}")]
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
        /// Retrieves accommodations of the specific contract
        /// </summary>
        /// <param name="contractId">ID of the contract</param>
        /// <returns>List of accommodations</returns>
        [HttpGet("{contractId}/accommodations")]
        [ProducesResponseType(typeof(Hiroshima.DirectManager.Models.Responses.Accommodation), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetContractAccommodations([FromRoute] int contractId)
        {
            var (_, isFailure, response, error) = await _accommodationManagementService.GetAccommodations(contractId);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response);
        }
        
        
        /// <summary>
        /// Retrieves all Contract Manager's accommodations
        /// </summary>
        /// <returns></returns>
        [HttpGet("accommodations")]
        [ProducesResponseType(typeof(List<Hiroshima.DirectManager.Models.Responses.Accommodation>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAllAccommodations([FromQuery] int skip = 0, [FromQuery] int top = 100)
        {
            var (_, isFailure, response, error) = await _accommodationManagementService.Get(skip, top);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response);
        }
        
        
        /// <summary>
        /// Creates a new accommodation
        /// </summary>
        /// <param name="accommodation">New accommodation data</param>
        /// <returns></returns>
        [HttpPost("accommodations")]
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
        [HttpPut("accommodations/{accommodationId}")]
        [ProducesResponseType(typeof(Hiroshima.DirectManager.Models.Responses.Accommodation), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateAccommodation([FromRoute] int accommodationId, [FromBody] Hiroshima.DirectManager.Models.Requests.Accommodation accommodation)
        {
            var (_, isFailure, response, error) = await _accommodationManagementService.Update(accommodationId, accommodation);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response);
        }
        
        
        /// <summary>
        /// Removes an accommodation by ID
        /// </summary>
        /// <param name="accommodationId">ID of the accommodation</param>
        /// <returns></returns>
        [HttpDelete("accommodations/{accommodationId}")]
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
        /// <param name="skip"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        [HttpGet("accommodations/{accommodationId}/rooms")]
        [ProducesResponseType(typeof(List<Hiroshima.DirectManager.Models.Responses.Room>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetRooms([FromRoute] int accommodationId, [FromQuery] int skip = 0, [FromQuery] int top = 100)
        { 
            var (_, isFailure, response, error) = await _accommodationManagementService.GetRooms(accommodationId, skip, top);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response);
        }


        /// <summary>
        /// Retrieves an accommodation room by Id
        /// </summary>
        /// <param name="accommodationId"></param>
        /// <param name="roomId"></param>
        /// <returns></returns>
        [HttpGet("accommodations/{accommodationId}/rooms/{roomId}")]
        [ProducesResponseType(typeof(Hiroshima.DirectManager.Models.Responses.Room), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetRoom([FromRoute] int accommodationId, [FromRoute] int roomId)
        { 
            var (_, isFailure, response, error) = await _accommodationManagementService.GetRoom(accommodationId, roomId);
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
        [HttpPost("accommodations/{accommodationId}/rooms")]
        [ProducesResponseType(typeof(List<Hiroshima.DirectManager.Models.Responses.Room>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddRooms([FromRoute] int accommodationId, [FromBody] List<Hiroshima.DirectManager.Models.Requests.Room> rooms)
        { 
            var (_, isFailure, response, error) = await _accommodationManagementService.AddRooms(accommodationId, rooms);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response);
        }


        /// <summary>
        /// Updates the room of the accommodation
        /// </summary>
        /// <param name="accommodationId"></param>
        /// <param name="roomId"></param>
        /// <param name="room"></param>
        /// <returns></returns>
        [HttpPut("accommodations/{accommodationId}/rooms/{roomId}")]
        [ProducesResponseType(typeof(Hiroshima.DirectManager.Models.Responses.Room), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateRoom([FromRoute] int accommodationId, [FromRoute] int roomId, [FromBody] Hiroshima.DirectManager.Models.Requests.Room room)
        { 
            var (_, isFailure, response, error) = await _accommodationManagementService.UpdateRoom(accommodationId, roomId, room);
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
        [HttpDelete("accommodations/{accommodationId}/rooms/{roomId}")]
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
        [HttpDelete("accommodations/{accommodationId}/rooms")]
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