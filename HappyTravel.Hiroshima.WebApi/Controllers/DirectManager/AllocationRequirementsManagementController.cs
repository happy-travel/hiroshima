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

        
        private readonly IAllocationRequirementManagementService _allocationRequirementManagementService;
    }
}