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
        

        private readonly IAvailabilityRestrictionsManagementService _availabilityRestrictionsManagementService;
    }
}