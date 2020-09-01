using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Common.Infrastructure;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms;
using HappyTravel.Hiroshima.DirectManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace HappyTravel.Hiroshima.WebApi.Controllers.DirectManager
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}/management")]
    [Produces("application/json")]
    public class AvailabilityRestrictionsManagementController : ControllerBase
    {
        public AvailabilityRestrictionsManagementController(IAvailabilityRestrictionsManagementService availabilityRestrictionsManagementService)
        {
            _availabilityRestrictionsManagementService = availabilityRestrictionsManagementService;
        }
        
        
        /// <summary>
        /// Replaces availability restrictions for the specific rooms. Removes previous, adds the new.
        /// </summary>
        /// <returns>List of the new availability restrictions</returns>
        [HttpPost("contracts/{contractId}/availability-restrictions")]
        [ProducesResponseType(typeof(List<Hiroshima.DirectManager.Models.Responses.AvailabilityRestriction>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> SetAvailabilityRestrictions([FromRoute] int contractId, [FromBody] List<HappyTravel.Hiroshima.DirectManager.Models.Requests.AvailabilityRestriction> availabilityRestrictions)
        {
            var (_, isFailure, response, error) = await _availabilityRestrictionsManagementService.Set(contractId, availabilityRestrictions);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response);
        }


        /// <summary>
        /// Retrieves availability restrictions
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="roomIds"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="restriction"></param>
        /// <param name="skip"></param>
        /// <param name="top"></param>
        /// <returns>List of availability restrictions</returns>
        [HttpGet("contracts/{contractId}/availability-restrictions")]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(List<Hiroshima.DirectManager.Models.Responses.AvailabilityRestriction>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetAvailabilityRestrictions([FromRoute] int contractId, [FromQuery(Name = "roomId")] List<int> roomIds, [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate, [FromQuery] AvailabilityRestrictions? restriction, [FromQuery] int skip = 0, [FromQuery] int top = 100)
        {
            var (_, isFailure, response, error) = await _availabilityRestrictionsManagementService.Get(contractId, skip, top, roomIds, fromDate, toDate, restriction);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response);
        }

        
        /// <summary>
        /// Removes availability restrictions
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="ids">Availability restriction ids</param>
        /// <returns></returns>
        [HttpDelete("contracts/{contractId}/availability-restrictions")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RemoveAvailabilityRestrictions([FromRoute] int contractId, [FromBody] List<int> ids)
        { 
            var (_, isFailure, error) = await _availabilityRestrictionsManagementService.Remove(contractId, ids);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return NoContent();
        }

        
        private readonly IAvailabilityRestrictionsManagementService _availabilityRestrictionsManagementService;
    }
}