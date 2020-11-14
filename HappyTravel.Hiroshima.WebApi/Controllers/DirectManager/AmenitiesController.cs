using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.DirectManager.Models.Responses;
using HappyTravel.Hiroshima.DirectManager.Services;
using HappyTravel.Hiroshima.WebApi.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HappyTravel.Hiroshima.WebApi.Controllers.DirectManager
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
            var languageCode = CultureInfo.CurrentCulture.Name;
            var (_, isFailure, response, error) = await _amenitiesService.Get(languageCode);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response);
        }


        private readonly IAmenityService _amenitiesService;
    }
}
