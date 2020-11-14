using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using HappyTravel.EdoContracts.GeoData;
using HappyTravel.EdoContracts.GeoData.Enums;
using HappyTravel.Hiroshima.DirectManager.Services;
using HappyTravel.Hiroshima.WebApi.Infrastructure.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace HappyTravel.Hiroshima.WebApi.Controllers.Connector
{
    [ApiController]
    [IgnoreLocalizationConvention]
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}/locations")]
    [Produces("application/json")]
    public class LocationsController : ControllerBase
    {
        public LocationsController(ILocationManagementService locationManagementService)
        {
            _locationManagementService = locationManagementService;
        }
        
        
        /// <summary>
        /// Retrieves locations for the Edo's updater
        /// </summary>
        /// <param name="modified"></param>
        /// <param name="locationType"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        [HttpGet("{modified}")]
        [ProducesResponseType(typeof(List<Location>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetLocations([FromRoute] DateTime modified, [FromQuery] LocationTypes locationType, [FromQuery] int skip = 0, [FromQuery] int take = 10000)
            => Ok(await _locationManagementService.Get(modified, locationType, skip, take));
        
        
        private readonly ILocationManagementService _locationManagementService;
    }
}