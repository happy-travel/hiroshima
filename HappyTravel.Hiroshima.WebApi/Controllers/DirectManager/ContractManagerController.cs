using System.Net;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.DirectManager.Services;
using HappyTravel.Hiroshima.WebApi.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace HappyTravel.Hiroshima.WebApi.Controllers.DirectManager
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}/management/contract-manager")]
    [Produces("application/json")]
    public class ContractManagerController : ControllerBase
    {
        public ContractManagerController(IContractManagerManagementService contractManagerManagementService, IdentityHttpClient identityHttpClient)
        {
            _contractManagerManagementService = contractManagerManagementService;
            _identityHttpClient = identityHttpClient;
        }
        
        
        /// <summary>
        /// Registers a contract manager
        /// </summary>
        /// <param name="contractManager"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(HappyTravel.Hiroshima.DirectManager.Models.Responses.Manager), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddContractManager([FromBody] Hiroshima.DirectManager.Models.Requests.Manager contractManager)
        {
            var (_, isFailure, response, error) = await GetEmailFromIdentity()
                .Bind(email => _contractManagerManagementService.Register(contractManager, email));
            
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response);
            
            
            async Task<Result<string>> GetEmailFromIdentity()
            {
                var email = await _identityHttpClient.GetEmail();
                
                return string.IsNullOrEmpty(email)
                    ? Result.Failure<string>("Failed to get the email claim")
                    : Result.Success(email);
            }
        }


        /// <summary>
        /// Retrieves current contract manager's data
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(HappyTravel.Hiroshima.DirectManager.Models.Responses.Manager), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetContractManager()
        {
            var (_, isFailure, response, error) = await _contractManagerManagementService.Get();
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));
            
            return Ok(response);
        }


        /// <summary>
        /// Modifies contract manager's data
        /// </summary>
        /// <param name="contractManager"></param>  
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(HappyTravel.Hiroshima.DirectManager.Models.Responses.Manager), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ModifyContractManager([FromBody] Hiroshima.DirectManager.Models.Requests.Manager contractManager)
        {
            var (_, isFailure, response, error) = await _contractManagerManagementService.Modify(contractManager);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response);
        }
    
        
        private readonly IdentityHttpClient _identityHttpClient;
        private readonly IContractManagerManagementService _contractManagerManagementService;
    }
}