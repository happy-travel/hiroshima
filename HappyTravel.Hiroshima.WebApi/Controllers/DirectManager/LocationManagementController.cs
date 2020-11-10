using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.EdoContracts.GeoData;
using HappyTravel.EdoContracts.GeoData.Enums;
using HappyTravel.Hiroshima.DirectManager.Services;
using HappyTravel.Hiroshima.WebApi.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace HappyTravel.Hiroshima.WebApi.Controllers.DirectManager
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}")]
    [Produces("application/json")]
    public class LocationManagementController : ControllerBase
    {
        public LocationManagementController(ILocationManagementService locationManagementService)
        {
            _locationManagementService = locationManagementService;
        }


        /// <summary>
        /// Adds location data or returns existed location
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        [HttpPost("management/locations")]
        [ProducesResponseType(typeof(Hiroshima.DirectManager.Models.Responses.Location), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddLocation([FromBody] Hiroshima.DirectManager.Models.Requests.Location location)
        {
            var (_, isFailure, response, error) = await _locationManagementService.Add(location);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response);
        }
        
        
        /// <summary>
        /// Retrieves all locations with paging
        /// </summary>
        /// <returns></returns>
        [HttpGet("management/locations")]
        [ProducesResponseType(typeof(List<Hiroshima.DirectManager.Models.Responses.Location>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetLocations([FromQuery] int skip = 0, [FromQuery] int top = 500) 
            => Ok(await _locationManagementService.Get(top, skip));
        
        
        /// <summary>
        /// Retrieves all country names with paging
        /// </summary>
        /// <param name="top"></param>
        /// <param name="skip"></param>
        /// <returns>Retrieves country names</returns>
        [HttpGet("management/locations/countries")]
        [ProducesResponseType(typeof(List<string>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetCountryNames([FromQuery] int skip = 0, [FromQuery] int top = 500) 
            => Ok(await _locationManagementService.GetCountryNames(top, skip));
      
        
        /// <summary>
        /// Retrieves locations for the Edo's updater
        /// </summary>
        /// <param name="modified"></param>
        /// <param name="locationType"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        [HttpGet("locations/{modified}")]
        [ProducesResponseType(typeof(List<Location>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetLocations([FromRoute] DateTime modified, [FromQuery] LocationTypes locationType, [FromQuery] int skip = 0, [FromQuery] int take = 10000)
            => Ok(await _locationManagementService.Get(modified, locationType, skip, take));
        
        
        private readonly ILocationManagementService _locationManagementService;
    }
}