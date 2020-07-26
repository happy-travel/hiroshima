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
    public class SeasonsController : Controller
    {
        public SeasonsController(ISeasonManagementService seasonManagementService)
        {
            _seasonManagementService = seasonManagementService;
        }

        
        /// <summary>
        /// Retrieves contract seasons 
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        [HttpGet("{contractId}/seasons")]
        [ProducesResponseType(typeof(List<Hiroshima.DirectManager.Models.Responses.Season>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetSeasons([FromRoute] int contractId)
        { 
            var (_, isFailure, response, error) = await _seasonManagementService.Get(contractId);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response);
        }


        /// <summary>
        /// Replaces previous seasons by the new
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="seasons"></param>
        /// <returns></returns>
        [HttpPost("{contractId}/seasons/replace")]
        [ProducesResponseType(typeof(List<Hiroshima.DirectManager.Models.Responses.Season>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ReplaceSeasons([FromRoute] int contractId, [FromBody] List<Hiroshima.DirectManager.Models.Requests.Season> seasons)
        {
            var (_, isFailure, response, error) = await _seasonManagementService.Replace(contractId, seasons);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response);
        }
        
        
        /// <summary>
        /// Removes seasons
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="ids">Season ids</param>
        /// <returns></returns>
        [HttpDelete("{contractId}/seasons")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RemoveSeasons([FromRoute] int contractId, [FromBody] List<int> ids)
        { 
            var (_, isFailure, error) = await _seasonManagementService.Remove(contractId, ids);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return NoContent();
        }

        
        private readonly ISeasonManagementService _seasonManagementService;
    }
}