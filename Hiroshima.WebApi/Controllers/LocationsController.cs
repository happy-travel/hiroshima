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
    [Route("api/{v:apiVersion}/dcLocations")]
    [Produces("application/json")]
    public class LocationsController : Controller
    {
        public LocationsController(IDcLocations dcLocations, IDcResponseConverter responseConverter)
        {
            _dcLocations = dcLocations;
            _dcResponseConverter = responseConverter;
        }


        [HttpGet]
        [ProducesResponseType(typeof(List<Location>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetLocations()
        {
            return Ok(_dcResponseConverter.CreateContractLocations(await _dcLocations.GetLocations()));
        }


        private readonly IDcLocations _dcLocations;
        private readonly IDcResponseConverter _dcResponseConverter;
    }
}
