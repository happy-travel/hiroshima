using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
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
    public class ImageManagementController : ControllerBase
    {
        public ImageManagementController(IImageManagementService imageManagementService)
        {
            _imageManagementService = imageManagementService;
        }


        /// <summary>
        /// Retrieves ordered list of images by accommodation ID
        /// </summary>
        /// <param name="accommodationId">ID of the accommodation</param>
        /// <returns></returns>
        [HttpGet("accommodations/{accommodationId}/photos")]
        [ProducesResponseType(typeof(List<Hiroshima.DirectManager.Models.Responses.SlimImage>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetImageList([FromRoute] int accommodationId)
        {
            var (_, isFailure, response, error) = await _imageManagementService.Get(accommodationId);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response);
        }


        /// <summary>
        /// Uploads image file
        /// </summary>
        /// <param name="accommodationId">Accommodation Id</param>
        /// <param name="uploadedFile">Adding image file</param>
        /// <returns>Image id</returns>
        [HttpPost("accommodations/{accommodationId}/photo")]
        [RequestSizeLimit(50 * 1024 * 1024)]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddImage([FromRoute] int accommodationId, [FromForm] IFormFile uploadedFile)
        {
            var image = new Hiroshima.DirectManager.Models.Requests.AccommodationImage
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
        /// Updates the list of images for accommodation by Id
        /// </summary>
        /// <param name="accommodationId">Accommodation Id</param>
        /// <param name="slimImages">Ordered list of images</param>
        /// <returns></returns>
        [HttpPut("accommodations/{accommodationId}/photos")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateAccommodationImages([FromRoute] int accommodationId, [FromBody] List<Hiroshima.DirectManager.Models.Requests.SlimImage> slimImages)
        {
            var (_, isFailure, error) = await _imageManagementService.Update(accommodationId, slimImages);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return NoContent();
        }


        /// <summary>
        /// Deletes image file by ID
        /// </summary>
        /// <param name="accommodationId">Accommodation Id</param>
        /// <param name="imageId">Id of the image file to be deleted</param>
        /// <returns></returns>
        [HttpDelete("accommodations/{accommodationId}/photos/{imageId}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RemoveImage([FromRoute] int accommodationId, [FromRoute] Guid imageId)
        {
            var (_, isFailure, error) = await _imageManagementService.Remove(accommodationId, imageId);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return NoContent();
        }


        /// <summary>
        /// Retrieves ordered list of images by accommodation ID and room ID
        /// </summary>
        /// <param name="accommodationId">ID of the accommodation</param>
        /// <param name="roomId">ID of the room</param>
        /// <returns></returns>
        [HttpGet("accommodations/{accommodationId}/rooms/{roomId}/photos")]
        [ProducesResponseType(typeof(List<Hiroshima.DirectManager.Models.Responses.SlimImage>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetImageList([FromRoute] int accommodationId, [FromRoute] int roomId)
        {
            var (_, isFailure, response, error) = await _imageManagementService.Get(accommodationId, roomId);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response);
        }


        /// <summary>
        /// Uploads image file for room
        /// </summary>
        /// <param name="accommodationId">Accommodation Id</param>
        /// <param name="roomId">Room Id</param>
        /// <param name="uploadedFile">Adding image file</param>
        /// <returns>Image id</returns>
        [HttpPost("accommodations/{accommodationId}/rooms/{roomId}/photo")]
        [RequestSizeLimit(50 * 1024 * 1024)]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddImage([FromRoute] int accommodationId, [FromRoute] int roomId, [FromForm] IFormFile uploadedFile)
        {
            var image = new Hiroshima.DirectManager.Models.Requests.RoomImage
            {
                AccommodationId = accommodationId,
                RoomId = roomId,
                UploadedFile = (FormFile)uploadedFile
            };
            var (_, isFailure, response, error) = await _imageManagementService.Add(image);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response);
        }


        /// <summary>
        /// Updates the list of images for room by Ids
        /// </summary>
        /// <param name="accommodationId">Accommodation Id</param>
        /// <param name="roomId">Room Id</param>
        /// <param name="slimImages">Ordered list of images</param>
        /// <returns></returns>
        [HttpPut("accommodations/{accommodationId}/rooms/{roomId}/photos")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateRoomImages([FromRoute] int accommodationId, [FromRoute] int roomId, [FromBody] List<Hiroshima.DirectManager.Models.Requests.SlimImage> slimImages)
        {
            var (_, isFailure, error) = await _imageManagementService.Update(accommodationId, roomId, slimImages);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return NoContent();
        }


        /// <summary>
        /// Deletes image file by ID
        /// </summary>
        /// <param name="accommodationId">Accommodation Id</param>
        /// <param name="roomId">Room Id</param>
        /// <param name="imageId">Id of the image file to be deleted</param>
        /// <returns></returns>
        [HttpDelete("accommodations/{accommodationId}/rooms/{roomId}/photos/{imageId}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RemoveImage([FromRoute] int accommodationId, [FromRoute] int roomId, [FromRoute] Guid imageId)
        {
            var (_, isFailure, error) = await _imageManagementService.Remove(accommodationId, roomId, imageId);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return NoContent();
        }


        private readonly IImageManagementService _imageManagementService;
    }
}
