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
    [Route("api/{v:apiVersion}/management")]
    [Produces("application/json")]
    public class AllocationRequirementsManagementController : ControllerBase
    {
        public AllocationRequirementsManagementController(IAllocationRequirementManagementService allocationRequirementManagementService)
        {
            _allocationRequirementManagementService = allocationRequirementManagementService;
        }

        
        /// <summary>
        /// Adds room allocation requirements according to a season
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="allocationRequirements"></param>
        /// <returns>List of room allocation requirements</returns>
        [HttpPost("contracts/{contractId}/allocation-requirements")]
        [ProducesResponseType(typeof(List<Hiroshima.DirectManager.Models.Responses.AllocationRequirement>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddAllocationRequirements(int contractId, [FromBody] List<Hiroshima.DirectManager.Models.Requests.AllocationRequirement> allocationRequirements)
        {
            var (_, isFailure, response, error) = await _allocationRequirementManagementService.Add(contractId, allocationRequirements);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response);
        }


        /// <summary>
        /// Retrieves room's allocation requirements. Query is used to filter requirements by room, season, season range
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="skip"></param>
        /// <param name="top"></param>
        /// <param name="roomIds"></param>
        /// <param name="seasonIds"></param>
        /// <param name="seasonRangeIds"></param>
        /// <returns>List of room allocation requirements</returns>
        [HttpGet("contracts/{contractId}/allocation-requirements")]
        [ProducesResponseType(typeof(List<Hiroshima.DirectManager.Models.Responses.AllocationRequirement>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAllocationRequirements([FromRoute] int contractId, [FromQuery] int skip = 0, [FromQuery] int top = 100, [FromQuery(Name = "roomId")] List<int> roomIds = null, [FromQuery(Name = "seasonId")] List<int> seasonIds = null, [FromQuery(Name = "seasonRangeId")] List<int> seasonRangeIds = null)
        {
            var (_, isFailure, response, error) = await _allocationRequirementManagementService.Get(contractId, skip, top, roomIds, seasonIds, seasonRangeIds);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response);
        }
        

        /// <summary>
        /// Removes room's allocation requirements by id
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="allocationRequirementIds"></param>
        /// <returns></returns>
        [HttpDelete("contracts/{contractId}/allocation-requirements")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RemoveAllocationRequirements([FromRoute] int contractId, [FromBody] List<int> allocationRequirementIds)
        {
            var (_, isFailure, error) = await _allocationRequirementManagementService.Remove(contractId, allocationRequirementIds);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return NoContent();
        }
        
        
        private readonly IAllocationRequirementManagementService _allocationRequirementManagementService;
    }
}