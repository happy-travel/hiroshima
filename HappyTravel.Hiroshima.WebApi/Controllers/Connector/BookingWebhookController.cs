using System.Net;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.EdoContracts.Accommodations;
using HappyTravel.Hiroshima.DirectManager.Services.Bookings;
using HappyTravel.Hiroshima.WebApi.Infrastructure.Attributes;
using HappyTravel.Hiroshima.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace HappyTravel.Hiroshima.WebApi.Controllers.Connector
{
    [ApiController]
    [IgnoreLocalizationConvention]
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}")]
    [Produces("application/json")]
    public class BookingWebhookController : BaseController
    {
        public BookingWebhookController(IBookingWebhookService bookingWebhookService, IBookingService bookingService)
        {
            _bookingWebhookService = bookingWebhookService;
            _bookingService = bookingService;
        }
        
        
        /// <summary>
        /// Processes a webhook request containing a booking status update
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(Booking), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        [HttpPost("bookings/response")]
        public async Task<IActionResult> ProcessBookingWebhookRequest()
        {
            var webhookDataResult = await _bookingWebhookService.Get(HttpContext.Request.Body);
            if (webhookDataResult.IsFailure)
                return BadRequest(webhookDataResult.Error);

            var (_, isFailure, booking, error) = await _bookingService.GetDetails(webhookDataResult.Value.Data.ReferenceCode);
            if (isFailure)
                return BadRequest(error);
            
            return Ok(booking);
        }


        private readonly IBookingWebhookService _bookingWebhookService;
        private readonly IBookingService _bookingService;
    }
}