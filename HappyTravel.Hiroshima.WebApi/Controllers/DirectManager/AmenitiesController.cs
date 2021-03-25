using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.DirectManager.Models.Responses;
using HappyTravel.Hiroshima.DirectManager.Services;
using HappyTravel.Hiroshima.Api.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HappyTravel.Hiroshima.Api.Controllers.DirectManager
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}")]
    [Produces("application/json")]
    public class AmenitiesController: ControllerBase
    {
        public AmenitiesController(IAmenityService amenitiesService)
        {
            _amenitiesService = amenitiesService;
        }


        /// <summary>
        /// Retrieves a list of all amenities for selected language
        /// </summary>
        /// <returns></returns>
        [HttpGet("amenities")]
        [ProducesResponseType(typeof(List<Amenity>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAmenities()
        {
            var (_, isFailure, response, error) = await _amenitiesService.Get(LanguageCode);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response);
        }


        /// <summary>
        /// Updates the list of all amenities and normalizes amenities in accommodations and rooms
        /// </summary>
        /// <returns></returns>
        [HttpPost("amenities/update")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateAmenities()
        {
            var (_, isFailure, error) = await _amenitiesService.NormalizeAllAmenitiesAndUpdateAmenitiesStore();
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok();
        }


        private string LanguageCode => CultureInfo.CurrentCulture.Name;


        private readonly IAmenityService _amenitiesService;
    }
}
