using System;
using System.Collections.Generic;
using Hiroshima.DbData.Models.Booking;
using Hiroshima.DbData.Models.Rates;

namespace Hiroshima.DbData.Models.Accommodation
{
    public class Season
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Accommodation Accommodation { get; set; }
        public int CancelationPolicyId { get; set; }
        public CancelationPolicy CancelationPolicy { get; set; }
        public int AccommodationId { get; set; }
        public ICollection<ContractedRate> ContractRates { get; set; }
    }
}