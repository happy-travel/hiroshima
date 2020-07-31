using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Common.Infrastructure;
using HappyTravel.Hiroshima.Common.Infrastructure.Utilities;
using HappyTravel.Hiroshima.DirectManager.Models.Requests;
using HappyTravel.Hiroshima.DirectManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace HappyTravel.Hiroshima.WebApi.Controllers.DirectManager
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}/management")]
    [Produces("application/json")]
    public class RatesManagementController : ControllerBase
    {
        public RatesManagementController(IRateManagementService rateManagementService)
        {
            _rateManagementService = rateManagementService;
        }


        /// <summary>
        /// Adds contract rates 
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="rates"></param>
        /// <returns></returns>
        [HttpPost("contracts/{contractId}/rates")]
        [ProducesResponseType(typeof(List<Hiroshima.DirectManager.Models.Responses.Rate>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddRates([FromRoute] int contractId, [FromBody] List<Rate> rates)
        { 
            var (_, isFailure, response, error) = await _rateManagementService.Add(contractId, rates);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response);
        }


        /// <summary>
        /// Retrieves contract rates
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="roomIds">List of room ids. E.g. roomIds = 1,2,3 </param>
        /// <param name="seasonIds">List of season ids. E.g. seasonIds = 1,2,3</param>
        /// <returns></returns>
        [HttpGet("contracts/{contractId}/rates")]
        [ProducesResponseType(typeof(List<Hiroshima.DirectManager.Models.Responses.Rate>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetRates([FromRoute] int contractId, [FromQuery] string? roomIds = null, [FromQuery] string? seasonIds = null)
        {
            var (_, isRoomIdsFailure, roomIdsResult, checkRoomIdsError) = QueryStringUtilities.GetIDs(roomIds!);
            if (isRoomIdsFailure)
                return BadRequest(ProblemDetailsBuilder.Build(checkRoomIdsError));
            
            var (_, isSeasonIdsFailure, seasonIdsResult, checkSeasonIdsError) = QueryStringUtilities.GetIDs(seasonIds!);
            if (isSeasonIdsFailure)
                return BadRequest(ProblemDetailsBuilder.Build(checkSeasonIdsError));
            
            var (_, isFailure, response, error) = await _rateManagementService.Get(contractId, roomIdsResult, seasonIdsResult);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response);
        }


        /// <summary>
        /// Removes contract rates
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="ids">Rate ids</param>
        /// <returns></returns>
        [HttpDelete("contracts/{contractId}/rates")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RemoveRates([FromRoute] int contractId, [FromBody] List<int> ids)
        { 
            var (_, isFailure, error) = await _rateManagementService.Remove(contractId, ids);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return NoContent();
        }
        
        
        private readonly IRateManagementService _rateManagementService;
    }
}