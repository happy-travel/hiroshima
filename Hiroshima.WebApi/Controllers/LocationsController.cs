using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using HappyTravel.EdoContracts.GeoData;
using Hiroshima.DirectContracts.Services;
using Hiroshima.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hiroshima.WebApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}/locations")]
    [Produces("application/json")]
    public class LocationsController : Controller
    {
        public LocationsController(IDirectContractsLocation directContractsLocation)
        {
            _directContractsLocation = directContractsLocation;
        }


        [HttpGet]
        [ProducesResponseType(typeof(List<Location>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetLocations()
        {
            return Ok(await _directContractsLocation.GetLocations());
        }


        private readonly IDirectContractsLocation _directContractsLocation;
    }
}
