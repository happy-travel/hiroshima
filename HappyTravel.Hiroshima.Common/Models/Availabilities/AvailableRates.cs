using System;
using System.Collections.Generic;
using System.Linq;

namespace HappyTravel.Hiroshima.Common.Models.Availabilities
{
    public class AvailableRates
    {
        public Guid Id { get; set; } = Guid.Empty;
        
        public int AccommodationId { get; set; }
        
        public string Hash { get; set; } = string.Empty;

        public List<RateDetails> Rates { get; set; } = new List<RateDetails>();

        public AvailableRatesSlim AvailableRatesSlim
            => new AvailableRatesSlim {Id = Id, Hash = Hash, AccommodationId = AccommodationId, Rates = Rates.Select(r => r.RateDetailsSlim).ToList()};
    }
}