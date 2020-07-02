using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using HappyTravel.EdoContracts.GeoData;
using HappyTravel.EdoContracts.GeoData.Enums;
using Hiroshima.DirectContracts.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hiroshima.WebApi.Controllers
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
        /// Returns locations to upload to Edo
        /// </summary>
        /// <param name="modified"></param>
        /// <param name="locationType"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpGet("{modified}")]
        [ProducesResponseType(typeof(List<Location>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetLocations([FromRoute] DateTime modified, [FromQuery] LocationTypes locationType, [FromQuery] int skip = 0, [FromQuery] int take = 10000)
        {
            throw new NotImplementedException();
        }


        private readonly ILocationService _locationService;
    }
}