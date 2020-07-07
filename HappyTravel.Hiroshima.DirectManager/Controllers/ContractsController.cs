using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.DirectManager.Models.Responses;
using HappyTravel.Hiroshima.DirectManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace HappyTravel.DirectManager.Controllers
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
        
        
        [HttpGet("{contractId}")]
        [ProducesResponseType(typeof(Models.Responses.Contract), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetContract([FromQuery] int contractId)
        {
            var (_, isFailure, response, error) = await  _contractManagementService.GetContract(contractId);
            if (isFailure)
                return BadRequest(error);

            return Ok(response);
        }
        
        
        [HttpGet]
        [ProducesResponseType(typeof(List<Models.Responses.Contract>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetContracts()
        {
            var (_, isFailure, response, error) = await _contractManagementService.GetContracts();
            if (isFailure)
                return BadRequest(error);

            return Ok(response);
        }
        
        
        [HttpPost]
        [ProducesResponseType(typeof(Models.Responses.Contract), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddContract([FromBody] Contract contract)
        {
            var (_, isFailure, response, error) = await _contractManagementService.AddContract(contract);
            if (isFailure)
                return BadRequest(error);

            return Ok(response);
        }
        
        
        [HttpPut("{contractId}")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateContract([FromRoute] int contractId, [FromBody] Contract contract)
        {
            var (_, isFailure, error) = await _contractManagementService.UpdateContract(contractId, contract);
            if (isFailure)
                return BadRequest(error);

            return Ok();
        }

        
        [HttpDelete("{contractId}")]
        [ProducesResponseType(typeof(Models.Responses.Contract), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteContract([FromRoute] int contractId)
        {
            var (_, isFailure, error) = await _contractManagementService.DeleteContract(contractId);
            if (isFailure)
                return BadRequest(error);

            return Ok();
        }
        
        
        private readonly IContractManagementService _contractManagementService;
    }
}