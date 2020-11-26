using System;
using System.Collections.Generic;

namespace HappyTravel.Hiroshima.Common.Models.Availabilities
{
    public class AvailableRatesSlim
    {
        public Guid Id { get; set; } = Guid.Empty;
        public int AccommodationId { get; set; }
        public string Hash { get; set; } = string.Empty;
        public List<RateDetailsSlim> Rates { get; set; } = new List<RateDetailsSlim>();
    }
}