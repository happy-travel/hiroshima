using System;
using System.Net;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Common.Infrastructure;
using HappyTravel.Hiroshima.DirectManager.Models.Responses;
using HappyTravel.Hiroshima.DirectManager.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HappyTravel.Hiroshima.WebApi.Controllers.DirectManager
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}/management/documents")]
    [Produces("application/json")]
    public class DocumentManagementController : ControllerBase
    {
        public DocumentManagementController(IDocumentManagementService documentManagementService)
        {
            _documentManagementService = documentManagementService;
        }

/*        /// <summary>
        /// Upload file of the contract
        /// </summary>
        /// <param name="contractId">Contract Id</param>
        /// <param name="document">Adding document</param>
        /// <returns></returns>
        [HttpPost("{contractId}")]
        [ProducesResponseType(typeof(Document), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddContractFile([FromRoute] int contractId, [FromBody] Hiroshima.DirectManager.Models.Requests.Document document)
        {
            var (_, isFailure, response, error) = await _documentManagementService.Add(document);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response);
        }*/

        /// <summary>
        /// Upload file of the contract
        /// </summary>
        /// <param name="contractId">Contract Id</param>
        /// <param name="uploadedFile">Adding contract file</param>
        /// <returns></returns>
        [HttpPost("{contractId}")]
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
        /// Delete file of the contract by ID
        /// </summary>
        /// <param name="contractId">Contract Id</param>
        /// <param name="documentId">Id of the file to be deleted</param>
        /// <returns></returns>
        [HttpDelete("{contractId}/{documentId}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RemoveContractFile([FromRoute] int contractId, [FromRoute] int documentId)
        {
            var (_, isFailure, error) = await _documentManagementService.Remove(contractId, documentId);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return NoContent();
        }

/*        /// <summary>
        /// Upload an image
        /// </summary>
        /// <param name="accommodationId">Accomodation Id</param>
        /// <param name="uploadedFile">Adding image</param>
        /// <returns></returns>
        [HttpPost("{accommodationId}")]
        [ProducesResponseType(typeof(Image), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddImageFile([FromRoute] int accommodationId, [FromBody] FormFile uploadedFile)
        {
            var image = new Hiroshima.DirectManager.Models.Requests.Image
            {
                AccommodationId = accommodationId,
                UploadedFile = uploadedFile
            };
            var (_, isFailure, response, error) = await _documentManagementService.AddImage(image);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response);
        }*/

/*        /// <summary>
        /// Delete image by ID
        /// </summary>
        /// <param name="contractId">Contract Id</param>
        /// <param name="documentId">Id of the file to be deleted</param>
        /// <returns></returns>
        [HttpDelete("{contractId}/{documentId}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RemoveImageFile([FromRoute] int contractId, [FromRoute] int documentId)
        {
            var (_, isFailure, error) = await _documentManagementService.Remove(contractId, documentId);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return NoContent();
        }*/

        private readonly IDocumentManagementService _documentManagementService;
    }
}
