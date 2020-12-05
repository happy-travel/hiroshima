using System;
using System.Collections.Generic;
using HappyTravel.Hiroshima.Common.Models.Enums;

namespace HappyTravel.Hiroshima.DirectManager.Models.Requests
{
    public class BookingRequest
    {
        public List<BookingStatuses> BookingStatuses { get; set; } = new List<BookingStatuses>
        {
            Common.Models.Enums.BookingStatuses.Processing,
            Common.Models.Enums.BookingStatuses.WaitingForCancellation,
            Common.Models.Enums.BookingStatuses.WaitingForCompletion,
            Common.Models.Enums.BookingStatuses.Rejected,
            Common.Models.Enums.BookingStatuses.Cancelled,
            Common.Models.Enums.BookingStatuses.Complete
        };
        
        public List<int> AccommodationIds { get; set; } = new List<int>();
        
        public DateTime FromDate { get; set; }
        
        public DateTime ToDate { get; set; }
    }
}