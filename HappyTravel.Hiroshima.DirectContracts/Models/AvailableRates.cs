using System;
using System.Collections.Generic;
namespace HappyTravel.Hiroshima.DirectContracts.Models
{
    public class AvailableRates
    {
        public Guid Id { get; set; } = Guid.Empty;
        public int AccommodationId { get; set; }
        public string Hash { get; set; } = string.Empty;
        public List<RateDetails> Rates { get; set; } = new List<RateDetails>();
    }
}