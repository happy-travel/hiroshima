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
    [Route("api/{v:apiVersion}/management/manager")]
    [Produces("application/json")]
    public class ManagerController : ControllerBase
    {
        public ManagerController(IManagerManagementService managerManagementService, IdentityHttpClient identityHttpClient)
        {
            _managerManagementService = managerManagementService;
            _identityHttpClient = identityHttpClient;
        }
        
        
        /// <summary>
        /// Registers a manager
        /// </summary>
        /// <param name="manager"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(HappyTravel.Hiroshima.DirectManager.Models.Responses.ManagerContext), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddManager([FromBody] Hiroshima.DirectManager.Models.Requests.Manager manager)
        {
            var (_, isFailure, response, error) = await GetEmailFromIdentity()
                .Bind(email => _managerManagementService.Register(manager, email));
            
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
        /// Registers a service supplier
        /// </summary>
        /// <param name="serviceSupplier"></param>
        /// <returns></returns>
        [HttpPost("service-supplier")]
        [ProducesResponseType(typeof(HappyTravel.Hiroshima.DirectManager.Models.Responses.ServiceSupplier), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddServiceSupplier([FromBody] Hiroshima.DirectManager.Models.Requests.ServiceSupplier serviceSupplier)
        {
            var (_, isFailure, response, error) = await _managerManagementService.RegisterServiceSupplier(serviceSupplier);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response);
        }


        /// <summary>
        /// Retrieves current manager's data
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(HappyTravel.Hiroshima.DirectManager.Models.Responses.ManagerContext), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetManager()
        {
            var (_, isFailure, response, error) = await _managerManagementService.Get();
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));
            
            return Ok(response);
        }


        /// <summary>
        /// Retrieves data for manager ID
        /// </summary>
        /// <returns></returns>
        [HttpGet("{managerId}")]
        [ProducesResponseType(typeof(HappyTravel.Hiroshima.DirectManager.Models.Responses.Manager), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetManager([FromRoute] int managerId)
        {
            var (_, isFailure, response, error) = await _managerManagementService.Get(managerId);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response);
        }


        /// <summary>
        /// Modifies manager's data
        /// </summary>
        /// <param name="manager"></param>  
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(HappyTravel.Hiroshima.DirectManager.Models.Responses.ManagerContext), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ModifyManager([FromBody] Hiroshima.DirectManager.Models.Requests.Manager manager)
        {
            var (_, isFailure, response, error) = await _managerManagementService.Modify(manager);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response);
        }


        /// <summary>
        /// Modifies manager's permissions
        /// </summary>
        /// <param name="managerId"></param>  
        /// <param name="managerPermissions"></param>  
        /// <returns></returns>
        [HttpPut("managerId/permissions")]
        [ProducesResponseType(typeof(HappyTravel.Hiroshima.DirectManager.Models.Responses.Manager), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ModifyManagerPermissions([FromRoute] int managerId, [FromBody] Hiroshima.DirectManager.Models.Requests.ManagerPermissions managerPermissions)
        {
            var (_, isFailure, response, error) = await _managerManagementService.ModifyPermissions(managerId, managerPermissions);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response);
        }


        private readonly IdentityHttpClient _identityHttpClient;
        private readonly IManagerManagementService _managerManagementService;
    }
}