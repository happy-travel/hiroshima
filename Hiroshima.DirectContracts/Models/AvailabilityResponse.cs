using System;
using System.Collections.Generic;

namespace Hiroshima.DirectContracts.Models
{
    public class AvailabilityResponse
    {
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int NumberOfNights => (int)(CheckOutDate - CheckInDate).TotalDays;
        public List<Accommodation> Accommodations
        {
            get => _accommodations ??= new List<Accommodation>();

            set => _accommodations = value;
        }
        private List<Accommodation> _accommodations;
    }
    
}
