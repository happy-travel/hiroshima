using System;
using System.Collections.Generic;

namespace Hiroshima.DirectContracts.Models.Responses
{
    public class DcAvailability
    {
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int NumberOfNights => (int)(CheckOutDate - CheckInDate).TotalDays;

        public List<DcAccommodation> Accommodations
        {
            get => _accommodations ??= new List<DcAccommodation>();

            set => _accommodations = value;
        }
        private List<DcAccommodation> _accommodations;
    }
    
}
