using System.Net;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Api.Infrastructure;
using HappyTravel.Hiroshima.Api.Infrastructure.Attributes;
using HappyTravel.Hiroshima.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace HappyTravel.Hiroshima.Api.Controllers.Connector
{
    [ApiController]
    [IgnoreLocalizationConvention]
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}/accommodations")]
    [Produces("application/json")]
    public class BookingController : BaseController
    {
        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }


        /// <summary>
        /// Creates a new booking order
        /// </summary>
        /// <param name="bookingRequest"></param>
        /// <returns>Booking order details</returns>
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

        
        /// <summary>
        /// Cancels the booking order by the reference code
        /// </summary>
        /// <param name="referenceCode"></param>
        /// <returns></returns>
        [HttpPost("bookings/{referenceCode}/cancel")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Cancel(string referenceCode)
        {
            var (_, isFailure, error) = await _bookingService.Cancel(referenceCode);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return NoContent();
        }


        /// <summary>
        /// Retrieves the booking order's details.
        /// </summary>
        /// <param name="referenceCode"></param>
        /// <returns></returns>
        [HttpGet("bookings/{referenceCode}")]
        [ProducesResponseType(typeof(EdoContracts.Accommodations.Booking), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetDetails(string referenceCode)
        {
            var (_, isFailure, bookingDetails, error) = await _bookingService.GetDetails(referenceCode);
            if (isFailure)
                return BadRequest(error);

            return Ok(bookingDetails);
        }

        
        private readonly IBookingService _bookingService;
    }
}