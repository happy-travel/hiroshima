using System;
using System.Net;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.DirectManager.Models.Responses;
using HappyTravel.Hiroshima.DirectManager.Services;
using HappyTravel.Hiroshima.Api.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HappyTravel.Hiroshima.Api.Controllers.DirectManager
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}/management/contracts")]
    [Produces("application/json")]
    public class DocumentManagementController : ControllerBase
    {
        public DocumentManagementController(IDocumentManagementService documentManagementService)
        {
            _documentManagementService = documentManagementService;
        }


        /// <summary>
        /// Returns a direct contract document file by ID
        /// </summary>
        /// <param name="contractId">ID of the contract</param>
        /// <param name="documentId">Uuid of the document</param>
        /// <returns></returns>
        [HttpGet("{contractId}/file/{documentId}")]
        [ProducesResponseType(typeof(DocumentFile), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetContractFile([FromRoute] int contractId, [FromRoute] Guid documentId)
        {
            var (_, isFailure, response, error) = await _documentManagementService.Get(contractId, documentId);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(File(response.FileBytes, response.ContentType, response.Name));
        }


        /// <summary>
        /// Uploads a document to a contract
        /// </summary>
        /// <param name="contractId">Contract Id</param>
        /// <param name="uploadedFile">Document file to add</param>
        /// <returns></returns>
        [HttpPost("{contractId}/file")]
        [RequestSizeLimit(100 * 1024 * 1024)]
        [ProducesResponseType(typeof(Document), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddContractFile([FromRoute] int contractId, [FromForm] IFormFile uploadedFile)
        {
            var document = new Hiroshima.DirectManager.Models.Requests.Document
            {
                ContractId = contractId,
                UploadedFile = (FormFile)uploadedFile
            };
            var (_, isFailure, response, error) = await _documentManagementService.Add(document);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response);
        }


        /// <summary>
        /// Deletes a document from a contract
        /// </summary>
        /// <param name="contractId">Contract Id</param>
        /// <param name="documentId">Document Id</param>
        /// <returns></returns>
        [HttpDelete("{contractId}/file/{documentId}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RemoveContractFile([FromRoute] int contractId, [FromRoute] Guid documentId)
        {
            var (_, isFailure, error) = await _documentManagementService.Remove(contractId, documentId);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return NoContent();
        }


        private readonly IDocumentManagementService _documentManagementService;
    }
}
