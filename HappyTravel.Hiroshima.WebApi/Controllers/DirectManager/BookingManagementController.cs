using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Common.Models.Enums;
using HappyTravel.Hiroshima.DirectManager.Models.Requests;
using HappyTravel.Hiroshima.DirectManager.Services;
using HappyTravel.Hiroshima.WebApi.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HappyTravel.Hiroshima.WebApi.Controllers.DirectManager
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}/contracts/accommodations/bookings")]
    [Produces("application/json")]
    public class BookingManagementController : BaseController
    {
        public BookingManagementController(IBookingManagementService bookingManagementService)
        {
            _bookingManagementService = bookingManagementService;
        }
        
        
        /// <summary>
        /// Retrieves booking orders
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<Hiroshima.DirectManager.Models.Responses.Bookings.BookingOrder>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Get([FromQuery] DateTime fromDate, [FromQuery] DateTime toDate, [FromQuery] List<BookingStatuses> bookingStatuses, [FromQuery] List<int> accommodationIds, [FromQuery] int skip = 0, [FromQuery] int top = 100)
        {
            var bookingRequest = SetBookingRequest();
            var (_, isFailure, response, error) = await _bookingManagementService.GetBookingOrders(bookingRequest, skip, top, LanguageCode);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response);


            BookingRequest SetBookingRequest()
            {
                if (!bookingStatuses.Any())
                {
                    bookingStatuses = new List<BookingStatuses>
                    {
                        BookingStatuses.Processing,
                        BookingStatuses.WaitingForCancellation,
                        BookingStatuses.WaitingForConfirmation,
                        BookingStatuses.Rejected,
                        BookingStatuses.Cancelled,
                        BookingStatuses.Confirmed
                    };
                }
                
                return new BookingRequest
                {
                    BookingStatuses = bookingStatuses,
                    AccommodationIds = accommodationIds,
                    FromDate = fromDate,
                    ToDate = toDate
                };
            }
        }

        
        /// <summary>
        /// Confirms a booking order
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        [HttpPost("{bookingId}/confirm")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Confirm([FromRoute] Guid bookingId)
        {
            var (_, isFailure, error) = await _bookingManagementService.ConfirmBooking(bookingId);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));
            
            return NoContent();
        }
        
        
        /// <summary>
        /// Confirms a booking order cancellation
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        [HttpPost("{bookingId}/cancel")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ConfirmCancellation([FromRoute] Guid bookingId)
        {
            var (_, isFailure, error) = await _bookingManagementService.ConfirmCancellation(bookingId);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));
            
            return NoContent();
        }
        
        private readonly IBookingManagementService _bookingManagementService;
    }
}