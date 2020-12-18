using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.DirectManager.Models.Requests;
using HappyTravel.Hiroshima.DirectManager.Services;
using HappyTravel.Hiroshima.WebApi.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace HappyTravel.Hiroshima.WebApi.Controllers.DirectManager
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}/management")]
    [Produces("application/json")]
    public class RateManagementController : ControllerBase
    {
        public RateManagementController(IRateManagementService rateManagementService)
        {
            _rateManagementService = rateManagementService;
        }


        /// <summary>
        /// Adds contract rates 
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="rates"></param>
        /// <returns>List of rates</returns>
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
        /// Modifies contract rate
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="rateId"></param>
        /// <param name="rate"></param>
        /// <returns>Modified rate</returns>
        [HttpPut("contracts/{contractId}/rates/{rateId}")]
        [ProducesResponseType(typeof(List<Hiroshima.DirectManager.Models.Responses.Rate>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ModifyRate([FromRoute] int contractId, [FromRoute] int rateId, [FromBody] Rate rate)
        { 
            var (_, isFailure, response, error) = await _rateManagementService.Modify(contractId, rateId, rate);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response);
        }
        
        
        /// <summary>
        /// Retrieves contract rates
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="skip"></param>
        /// <param name="top"></param>
        /// <param name="roomIds">List of room ids</param>
        /// <param name="seasonIds">List of season ids</param>
        /// <returns>List of rates</returns>
        [HttpGet("contracts/{contractId}/rates")]
        [ProducesResponseType(typeof(List<Hiroshima.DirectManager.Models.Responses.Rate>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetRates([FromRoute] int contractId, [FromQuery] int skip = 0, [FromQuery] int top = 100, [FromQuery(Name = "roomId")] List<int> roomIds = null, [FromQuery(Name = "seasonId")] List<int> seasonIds = null)
        {
            var (_, isFailure, response, error) = await _rateManagementService.Get(contractId, skip, top, roomIds, seasonIds);
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