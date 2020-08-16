using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Common.Infrastructure;
using HappyTravel.Hiroshima.DirectManager.Models.Common;
using HappyTravel.Hiroshima.DirectManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace HappyTravel.Hiroshima.WebApi.Controllers.DirectManager
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}/management")]
    [Produces("application/json")]
    public class CancellationPolicyController : ControllerBase
    {
        public CancellationPolicyController(ICancellationPolicyManagementService cancellationPolicyManagementService)
        {
            _cancellationPolicyManagementService = cancellationPolicyManagementService;
        }

        
        /// <summary>
        /// Adds contract cancellation policies 
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="cancellationPolicies"></param>
        /// <returns></returns>
        [HttpPost("contracts/{contractId}/cancellation-policies")]
        [ProducesResponseType(typeof(List<Hiroshima.DirectManager.Models.Responses.CancellationPolicy>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddCancellationPolicies([FromRoute] int contractId, [FromBody] List<Hiroshima.DirectManager.Models.Requests.CancellationPolicy> cancellationPolicies)
        { 
            var (_, isFailure, response, error) = await _cancellationPolicyManagementService.Add(contractId, cancellationPolicies);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response);
        }
        

        /// <summary>
        /// Retrieves contract cancellation policies
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="roomIds">List of room ids</param>
        /// <param name="seasonIds">List of season ids</param>
        /// <returns></returns>
        [HttpGet("contracts/{contractId}/cancellation-policies")]
        [ProducesResponseType(typeof(List<Hiroshima.DirectManager.Models.Responses.CancellationPolicy>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetCancellationPolicies([FromRoute] int contractId, [FromQuery(Name = "roomId")] List<int> roomIds = null, [FromQuery(Name = "seasonId")] List<int> seasonIds = null)
        {
            var (_, isFailure, response, error) = await _cancellationPolicyManagementService.Get(contractId, roomIds, seasonIds);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response);
        }
        
        
        /// <summary>
        /// Removes contract cancellation policies
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="ids">Cancellation policy ids</param>
        /// <returns></returns>
        [HttpDelete("contracts/{contractId}/cancellation-policies")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RemoveCancellationPolicies([FromRoute] int contractId, [FromBody] Identifiers ids)
        { 
            var (_, isFailure, error) = await _cancellationPolicyManagementService.Remove(contractId, ids.Ids);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return NoContent();
        }
        
        
        private readonly ICancellationPolicyManagementService _cancellationPolicyManagementService;
    }
}