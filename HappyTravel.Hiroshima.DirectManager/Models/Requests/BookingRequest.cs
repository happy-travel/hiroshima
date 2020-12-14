using System;
using System.Collections.Generic;
using HappyTravel.Hiroshima.Common.Models.Enums;

namespace HappyTravel.Hiroshima.DirectManager.Models.Requests
{
    public class BookingRequest
    {
        public List<BookingStatuses> BookingStatuses { get; set; }
        
        public List<int> AccommodationIds { get; set; } = new List<int>();
        
        public DateTime FromDate { get; set; }
        
        public DateTime ToDate { get; set; }
    }
}