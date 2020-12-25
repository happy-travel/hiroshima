using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.DirectManager.Models.Responses;
using HappyTravel.Hiroshima.DirectManager.Services;
using HappyTravel.Hiroshima.WebApi.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace HappyTravel.Hiroshima.WebApi.Controllers.DirectManager
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}/management")]
    [Produces("application/json")]
    public class ManagerController : ControllerBase
    {
        public ManagerController(IManagerManagementService managerManagementService, IManagerRegistrationService managerRegistrationService, 
            IManagerInvitationService managerInvitationService, IdentityHttpClient identityHttpClient)
        {
            _managerManagementService = managerManagementService;
            _managerRegistrationService = managerRegistrationService;
            _managerInvitationService = managerInvitationService;
            _identityHttpClient = identityHttpClient;
        }


        /// <summary>
        /// Retrieves current manager's data
        /// </summary>
        /// <returns></returns>
        [HttpGet("manager")]
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
        [HttpGet("manager/{managerId}")]
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
        [HttpGet("manager/service-suppliers")]
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
        [HttpPost("manager/register-master")]
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
        [HttpPost("manager/register")]
        [ProducesResponseType(typeof(HappyTravel.Hiroshima.DirectManager.Models.Responses.ManagerContext), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RegisterInvitedManager([FromBody] Hiroshima.DirectManager.Models.Requests.ManagerInfoWithCode managerInfo)
        {
            var (_, isFailure, response, error) = await GetEmailFromIdentity()
                .Bind(email => _managerRegistrationService.RegisterInvited(managerInfo, email));

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
        ///     Sends an invite to a regular manager
        /// </summary>
        /// <param name="managerInvitation">Regular manager registration request.</param>
        /// <returns></returns>
        [HttpPost("manager/invitations/send")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> InviteManager([FromBody] Hiroshima.DirectManager.Models.Requests.ManagerInvitationInfo managerInvitation)
        {
            var (_, isFailure, error) = await _managerInvitationService.Send(managerInvitation);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return NoContent();
        }


        /// <summary>
        ///    Resend manager invitation
        /// </summary>
        /// <param name="invitationCode">Invitation code</param>>
        [HttpPost("manager/invitations/{invitationCode}/resend")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ResendInvite([FromRoute] string invitationCode)
        {
            var (_, isFailure, error) = await _managerInvitationService.Resend(invitationCode);
            if (isFailure)
                return BadRequest(error);

            return NoContent();
        }


        /// <summary>
        ///     Creates invitation for regular manager.
        /// </summary>
        /// <param name="managerInvitation">Regular manager registration request.</param>
        /// <returns>Invitation code.</returns>
        [HttpPost("manager/invitations/generate")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateInvitation([FromBody] Hiroshima.DirectManager.Models.Requests.ManagerInvitationInfo managerInvitation)
        {
            var (_, isFailure, invitationCode, error) = await _managerInvitationService.Create(managerInvitation);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(invitationCode);
        }


        /// <summary>
        ///     Gets invitation data.
        /// </summary>
        /// <param name="invitationCode">Invitation code.</param>
        /// <returns>Invitation data, including pre-filled registration information.</returns>
        [HttpGet("manager/invitations/{invitationCode}")]
        [ProducesResponseType(typeof(ManagerInvitation), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetInvitationData(string invitationCode)
        {
            var (_, isFailure, invitationInfo, error) = await _managerInvitationService.GetPendingInvitation(invitationCode);

            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(invitationInfo);
        }


        /// <summary>
        ///    Gets not accepted invitations list for service supplier
        /// </summary>
        /// <returns>List not accepted invitations for service supplier</returns>
        [HttpGet("service-supplier/invitations")]
        [ProducesResponseType(typeof(List<ManagerInvitation>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<ManagerInvitation>>> GetServiceSupplierInvitations()
        {
            var (_, isFailure, response, error) = await _managerInvitationService.GetServiceSupplierInvitations();
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response);
        }


        /// <summary>
        /// Updates service supplier data
        /// </summary>
        /// <param name="serviceSupplier"></param>
        /// <returns></returns>
        [HttpPut("manager/service-supplier")]
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
        [HttpPut("manager")]
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
        [HttpPut("manager/{managerId}/permissions")]
        [ProducesResponseType(typeof(HappyTravel.Hiroshima.DirectManager.Models.Responses.ManagerContext), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ModifyManagerPermissions([FromRoute] int managerId, [FromBody] Hiroshima.DirectManager.Models.Requests.Permissions managerPermissions)
        {
            var (_, isFailure, response, error) = await _managerManagementService.ModifyPermissions(managerId, managerPermissions);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response);
        }


        /// <summary>
        /// Transfer master manager rights to regular manager
        /// </summary>
        /// <param name="managerId"></param>  
        /// <returns></returns>
        [HttpPut("manager/{managerId}/transfer-master")]
        [ProducesResponseType(typeof(HappyTravel.Hiroshima.DirectManager.Models.Responses.ManagerContext), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> TransferMasterManagerPermissions([FromRoute] int managerId)
        {
            var (_, isFailure, error) = await _managerRegistrationService.TransferMaster(managerId);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return NoContent();
        }


        private readonly IdentityHttpClient _identityHttpClient;
        private readonly IManagerManagementService _managerManagementService;
        private readonly IManagerRegistrationService _managerRegistrationService;
        private readonly IManagerInvitationService _managerInvitationService;
    }
}