using System;
using System.Net;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.DirectManager.Models.Responses;
using HappyTravel.Hiroshima.DirectManager.Services;
using HappyTravel.Hiroshima.WebApi.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HappyTravel.Hiroshima.WebApi.Controllers.DirectManager
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}/management/contracts")]
    [Produces("application/json")]
    public class ImageManagementController : ControllerBase
    {
        public ImageManagementController(IImageManagementService imageManagementService)
        {
            _imageManagementService = imageManagementService;
        }


        /// <summary>
        /// Uploads image file
        /// </summary>
        /// <param name="accommodationId">Accommodation Id</param>
        /// <param name="uploadedFile">Adding image file</param>
        /// <returns></returns>
        [HttpPost("accommodations/{accommodationId}/photo")]
        [RequestSizeLimit(50 * 1024 * 1024)]
        [ProducesResponseType(typeof(Image), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddImageFile([FromRoute] int accommodationId, [FromForm] IFormFile uploadedFile)
        {
            var image = new Hiroshima.DirectManager.Models.Requests.Image
            {
                AccommodationId = accommodationId,
                UploadedFile = (FormFile)uploadedFile
            };
            var (_, isFailure, response, error) = await _imageManagementService.Add(image);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response);
        }


        /// <summary>
        /// Deletes image file by ID
        /// </summary>
        /// <param name="accommodationId">Accommodation Id</param>
        /// <param name="imageId">Id of the image file to be deleted</param>
        /// <returns></returns>
        [HttpDelete("accommodations/{accommodationId}/photo/{imageId}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RemoveImageFile([FromRoute] int accommodationId, [FromRoute] Guid imageId)
        {
            var (_, isFailure, error) = await _imageManagementService.Remove(accommodationId, imageId);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return NoContent();
        }


        private readonly IImageManagementService _imageManagementService;
    }
}
