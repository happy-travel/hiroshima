using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Common.Infrastructure;
using HappyTravel.Hiroshima.DirectManager.Models.Responses;
using HappyTravel.Hiroshima.DirectManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace HappyTravel.Hiroshima.DirectManager.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}/management/contracts")]
    [Produces("application/json")]
    public class ContractsController : Controller
    {
        public ContractsController(IContractManagementService contractManagementService)
        {
            _contractManagementService = contractManagementService;
        }
        
        
        /// <summary>
        /// Returns a direct contract data by ID
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        [HttpGet("{contractId}")]
        [ProducesResponseType(typeof(Contract), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetContract([FromQuery] int contractId)
        {
            var (_, isFailure, response, error) = await _contractManagementService.GetContract(contractId);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response);
        }
        
        
        /// <summary>
        /// Gets all user's contracts
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<Contract>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetContracts()
        {
            var (_, isFailure, response, error) = await _contractManagementService.GetContracts();
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response);
        }
        
        
        /// <summary>
        /// Adds a new contract
        /// </summary>
        /// <param name="contract"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Contract), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddContract([FromBody] Models.Requests.Contract contract)
        {
            var (_, isFailure, response, error) = await _contractManagementService.AddContract(contract);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response);
        }
        
        
        /// <summary>
        /// Updates a contract by ID
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="contract"></param>
        /// <returns></returns>
        [HttpPut("{contractId}")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateContract([FromRoute] int contractId, [FromBody] Models.Requests.Contract contract)
        {
            var (_, isFailure, error) = await _contractManagementService.UpdateContract(contractId, contract);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok();
        }

        
        /// <summary>
        /// Deletes a contract by ID
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        [HttpDelete("{contractId}")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteContract([FromRoute] int contractId)
        {
            var (_, isFailure, error) = await _contractManagementService.DeleteContract(contractId);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok();
        }
        
        
        private readonly IContractManagementService _contractManagementService;
    }
}