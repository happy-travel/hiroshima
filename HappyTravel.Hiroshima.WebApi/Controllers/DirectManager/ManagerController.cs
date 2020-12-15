using System.Collections.Generic;
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
        public ManagerController(IManagerManagementService managerManagementService, IManagerRegistrationService managerRegistrationService, 
            IdentityHttpClient identityHttpClient)
        {
            _managerManagementService = managerManagementService;
            _managerRegistrationService = managerRegistrationService;
            _identityHttpClient = identityHttpClient;
        }


        /// <summary>
        /// Retrieves current manager's data
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(HappyTravel.Hiroshima.DirectManager.Models.Responses.ManagerContext), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
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
        [ProducesResponseType(typeof(HappyTravel.Hiroshima.DirectManager.Models.Responses.ManagerContext), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetManager([FromRoute] int managerId)
        {
            var (_, isFailure, response, error) = await _managerManagementService.Get(managerId);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response);
        }


        /// <summary>
        /// Retrieves current manager's service supplers list
        /// </summary>
        /// <returns></returns>
        [HttpGet("service-suppliers")]
        [ProducesResponseType(typeof(List<HappyTravel.Hiroshima.DirectManager.Models.Responses.ServiceSupplier>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetServiceSuppliers()
        {
            var (_, isFailure, response, error) = await _managerManagementService.GetServiceSuppliers();
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response);
        }


        /// <summary>
        /// Registers master manager with service supplier
        /// </summary>
        /// <param name="managerWithServiceSupplier">Master manager with service supplier request</param>
        /// <returns></returns>
        [HttpPost("register-master")]
        [ProducesResponseType(typeof(HappyTravel.Hiroshima.DirectManager.Models.Responses.ManagerContext), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RegisterManagerWithServiceSupplier([FromBody] Hiroshima.DirectManager.Models.Requests.ManagerWithServiceSupplier managerWithServiceSupplier)
        {
            var (_, isFailure, response, error) = await GetEmailFromIdentity()
                .Bind(email => _managerRegistrationService.RegisterWithServiceSupplier(managerWithServiceSupplier, email));
            
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
        /// Registers regular manager
        /// </summary>
        /// <param name="managerInfo">Invited manager info</param>
        /// <returns></returns>
        [HttpPost("register")]
        [ProducesResponseType(typeof(HappyTravel.Hiroshima.DirectManager.Models.Responses.ManagerContext), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RegisterInvitedManager([FromBody] Hiroshima.DirectManager.Models.Requests.ManagerInfo managerInfo)
        {
            var (_, isFailure, response, error) = await GetEmailFromIdentity()
                .Bind(email => _managerRegistrationService.RegisterInvited(managerInfo, "", email));

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
        /// Updates service supplier data
        /// </summary>
        /// <param name="serviceSupplier"></param>
        /// <returns></returns>
        [HttpPut("service-supplier")]
        [ProducesResponseType(typeof(HappyTravel.Hiroshima.DirectManager.Models.Responses.ServiceSupplier), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddServiceSupplier([FromBody] Hiroshima.DirectManager.Models.Requests.ServiceSupplier serviceSupplier)
        {
            var (_, isFailure, response, error) = await _managerManagementService.ModifyServiceSupplier(serviceSupplier);
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
        [HttpPut("{managerId}/permissions")]
        [ProducesResponseType(typeof(HappyTravel.Hiroshima.DirectManager.Models.Responses.ManagerContext), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ModifyManagerPermissions([FromRoute] int managerId, [FromBody] Hiroshima.DirectManager.Models.Requests.Permissions managerPermissions)
        {
            var (_, isFailure, response, error) = await _managerManagementService.ModifyPermissions(managerId, managerPermissions);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response);
        }


        private readonly IdentityHttpClient _identityHttpClient;
        private readonly IManagerManagementService _managerManagementService;
        private readonly IManagerRegistrationService _managerRegistrationService;
    }
}