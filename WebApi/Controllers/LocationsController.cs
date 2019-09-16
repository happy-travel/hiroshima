using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using HappyTravel.EdoContracts.GeoData;
using HappyTravel.EdoContracts.GeoData.Enums;
using HappyTravel.Geography;
using Hiroshima.DirectContracts;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services;

namespace WebApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}/locations")]
    [Produces("application/json")]
    public class LocationsController : Controller
    {
        public LocationsController(IDirectContracts directContracts, IDcResponseConverter responseConverter)
        {
            _directContracts = directContracts;
            _responseConverter = responseConverter;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<Location>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetLocations()
        {
            return Ok(_responseConverter.CreateContractLocations(await _directContracts.Locations.GetLocations()));
        }

        private readonly IDirectContracts _directContracts;
        private readonly IDcResponseConverter _responseConverter;

    }
}
