using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.WebApi.Infrastructure;
using HappyTravel.Hiroshima.WebApi.Infrastructure.Attributes;
using HappyTravel.Hiroshima.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace HappyTravel.Hiroshima.WebApi.Controllers.Connector
{
    [ApiController]
    [IgnoreLocalizationConvention]
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}/accommodations")]
    [Produces("application/json")]
    public class BookingController : Controller
    {
        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }


        /// <summary>
        /// Makes an asynchronous booking request to the Etg api.
        /// </summary>
        /// <param name="bookingRequest"></param>
        /// <returns></returns>
        [HttpPost("bookings")]
        [ProducesResponseType(typeof(EdoContracts.Accommodations.Booking), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Book([FromBody] EdoContracts.Accommodations.BookingRequest bookingRequest)
        {
            var (_, isFailure, bookingDetails, error) = await _bookingService.Book(bookingRequest, LanguageCode);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(bookingDetails);
        }


        private readonly IBookingService _bookingService;
        private string LanguageCode => CultureInfo.CurrentCulture.Name;
    }
}