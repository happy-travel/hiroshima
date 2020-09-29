using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Common.Infrastructure;
using HappyTravel.Hiroshima.DirectManager.Models.Responses;
using HappyTravel.Hiroshima.DirectManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace HappyTravel.Hiroshima.WebApi.Controllers.DirectManager
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}/management/contracts")]
    [Produces("application/json")]
    public class ContractManagementController : ControllerBase
    {
        public ContractManagementController(IContractManagementService contractManagementService)
        {
            _contractManagementService = contractManagementService;
        }


        /// <summary>
        /// Returns a direct contract data by ID
        /// </summary>
        /// <param name="contractId">ID of the contract</param>
        /// <returns></returns>
        [HttpGet("{contractId}")]
        [ProducesResponseType(typeof(Contract), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetContract([FromRoute] int contractId)
        {
            var (_, isFailure, response, error) = await _contractManagementService.Get(contractId);
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
        public async Task<IActionResult> GetContracts([FromQuery] int skip = 0, [FromQuery] int top = 100)
        {
            var (_, isFailure, response, error) = await _contractManagementService.GetContracts(skip, top);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response);
        }


        /// <summary>
        /// Adds a new contract
        /// </summary>
        /// <param name="contract">New contract data</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Contract), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddContract([FromBody] Hiroshima.DirectManager.Models.Requests.Contract contract)
        {
            var (_, isFailure, response, error) = await _contractManagementService.Add(contract);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response);
        }


        /// <summary>
        /// Updates a contract by ID
        /// </summary>
        /// <param name="contractId">ID of the contract</param>
        /// <param name="contract">New contract data</param>
        /// <returns></returns>
        [HttpPut("{contractId}")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateContract([FromRoute] int contractId, [FromBody] Hiroshima.DirectManager.Models.Requests.Contract contract)
        {
            var (_, isFailure, error) = await _contractManagementService.Update(contractId, contract);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return NoContent();
        }


        /// <summary>
        /// Removes a contract by ID
        /// </summary>
        /// <param name="contractId">New contract data</param>
        /// <returns></returns>
        [HttpDelete("{contractId}")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RemovesContract([FromRoute] int contractId)
        {
            var (_, isFailure, error) = await _contractManagementService.Remove(contractId);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return NoContent();
        }


        /// <summary>
        /// Upload file of the contract
        /// </summary>
        /// <param name="contractId">Contract Id</param>
        /// <param name="document">Adding document</param>
        /// <returns></returns>
        [HttpPost("{contractId}/file")]
        [ProducesResponseType(typeof(Contract), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddContractFile([FromRoute] int contractId, [FromBody] Hiroshima.DirectManager.Models.Requests.Document document)
        {
            var (_, isFailure, response, error) = await _documentManagementService.Add(document);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response); 
        }

        /// <summary>
        /// Delete file of the contract by ID
        /// </summary>
        /// <param name="contractId">Contract Id</param>
        /// <param name="documentId">Id of the file to be deleted</param>
        /// <returns></returns>
        [HttpDelete("{contractId}/file/{DocumentId}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RemoveContractFile([FromRoute] int contractId, [FromRoute] int documentId)
        {
            var (_, isFailure, error) = await _documentManagementService.Remove(contractId, documentId);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return NoContent();
        }

        private readonly IContractManagementService _contractManagementService;
        private readonly IDocumentManagementService _documentManagementService;
    }
}