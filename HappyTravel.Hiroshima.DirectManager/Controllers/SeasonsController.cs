using Microsoft.AspNetCore.Mvc;

namespace HappyTravel.DirectManager.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}/management/contracts/seasons")]
    [Produces("application/json")]
    public class SeasonsController : Controller
    {
        public SeasonsController()
        {
            
        }
    }
}