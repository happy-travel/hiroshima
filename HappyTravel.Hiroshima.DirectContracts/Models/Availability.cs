using System.Collections.Generic;
using HappyTravel.Hiroshima.Common.Models.Accommodations;

namespace HappyTravel.Hiroshima.DirectContracts.Models
{
    public class Availability
    {
        public string Id { get; set; }
        public Dictionary<Accommodation, List<AvailableRates>> AvailableRates { get; set; }
    }
}