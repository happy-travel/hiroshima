using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using HappyTravel.EdoContracts.GeoData;
using HappyTravel.EdoContracts.GeoData.Enums;
using HappyTravel.Hiroshima.DirectContracts.Services;
using Microsoft.AspNetCore.Mvc;

namespace HappyTravel.Hiroshima.WebApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}/locations")]
    [Produces("application/json")]
    public class LocationsController : Controller
    {
        public LocationsController(ILocationService locationService)
        {
            _locationService = locationService;
        }


        /// <summary>
        /// Returns available locations
        /// </summary>
        [HttpGet("{modified}")]
        [ProducesResponseType(typeof(List<Location>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetLocations([FromRoute] DateTime modified, [FromQuery] LocationTypes locationType, [FromQuery] int skip = 0, [FromQuery] int take = 10000)
        {
            throw new NotImplementedException();
        }


        private readonly ILocationService _locationService;
    }
}