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
    [Route("api/{v:apiVersion}/management/contracts")]
    [Produces("application/json")]
    public class SeasonManagementController : ControllerBase
    {
        public SeasonManagementController(ISeasonManagementService seasonManagementService)
        {
            _seasonManagementService = seasonManagementService;
        }
        

        /// <summary>
        /// Adds seasons to the contract
        /// </summary>
        /// <param name="contractId">The id of a contract</param>
        /// <param name="names">Season names</param>
        /// <returns>Season names with id</returns>
        [HttpPost("{contractId}/seasons")]
        [ProducesResponseType(typeof(List<Hiroshima.DirectManager.Models.Responses.Season>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddSeasons([FromRoute] int contractId, [FromBody] List<string> names)
        {
            var (_, isFailure, response, error) = await _seasonManagementService.Add(contractId, names);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response);
        }


        /// <summary>
        /// Retrieves seasons 
        /// </summary>
        /// <param name="contractId">The id of a contract</param>
        /// <param name="skip"></param>
        /// <param name="top"></param>
        /// <returns>Season names with id</returns>
        [HttpGet("{contractId}/seasons")]
        [ProducesResponseType(typeof(List<Hiroshima.DirectManager.Models.Responses.Season>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetSeasons([FromRoute] int contractId, [FromQuery] int skip = 0, [FromQuery] int top = 100)
        { 
            var (_, isFailure, response, error) = await _seasonManagementService.Get(contractId, skip, top);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response);
        }


        /// <summary>
        /// Removes an empty season, i.e. the season without associated date ranges.
        /// </summary>
        /// <param name="contractId">The id of a contract</param>
        /// <param name="seasonId">Season id</param>
        /// <returns></returns>
        [HttpDelete("{contractId}/seasons/{seasonId}")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RemoveSeason([FromRoute] int contractId, [FromRoute] int seasonId)
        { 
            var (_, isFailure, error) = await _seasonManagementService.Remove(contractId, seasonId);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return NoContent();
        }
        
        
        /// <summary>
        /// Sets season's date ranges
        /// </summary>
        /// <param name="contractId">The id of a contract</param>
        /// <param name="seasonRanges">A list of date ranges that belong to contractes seasons</param>
        /// <returns></returns>
        [HttpPost("{contractId}/seasons/ranges")]
        [ProducesResponseType(typeof(List<Hiroshima.DirectManager.Models.Responses.SeasonRange>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> SetSeasonRanges([FromRoute] int contractId, [FromBody] List<Hiroshima.DirectManager.Models.Requests.SeasonRange> seasonRanges)
        {
            var (_, isFailure, response, error) = await _seasonManagementService.SetSeasonRanges(contractId, seasonRanges);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response);
        }


        /// <summary>
        /// Retrieves contract's season ranges
        /// </summary>
        /// <param name="contractId">The id of a contract</param>
        /// <param name="skip"></param>
        /// <param name="top"></param>
        /// <returns>A range of dates that belong to some season with start and end date</returns>
        [HttpGet("{contractId}/seasons/ranges")]
        [ProducesResponseType(typeof(List<Hiroshima.DirectManager.Models.Responses.SeasonRange>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetSeasonRanges([FromRoute] int contractId, [FromQuery] int skip = 0, [FromQuery] int top = 100)
        {
            var (_, isFailure, response, error) = await _seasonManagementService.GetSeasonRanges(contractId, skip, top);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response);
        }


        /// <summary>
        /// Retrieves contract's season ranges of the specific season
        /// </summary>
        /// <param name="contractId">The id of a contract</param>
        /// <param name="seasonId">The id of a season</param>
        /// <param name="skip"></param>
        /// <param name="top"></param>
        /// <returns>A range of dates that belong to some season with start and end date</returns>
        [HttpGet("{contractId}/seasons/{seasonId}/ranges")]
        [ProducesResponseType(typeof(List<Hiroshima.DirectManager.Models.Responses.SeasonRange>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetDateRangesOfTheSeason([FromRoute] int contractId, [FromRoute] int seasonId, [FromQuery] int skip = 0, [FromQuery] int top = 100)
        {
            var (_, isFailure, response, error) = await _seasonManagementService.GetSeasonRanges(contractId, seasonId, skip, top);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response);
        }
        
        
        private readonly ISeasonManagementService _seasonManagementService;
    }
}