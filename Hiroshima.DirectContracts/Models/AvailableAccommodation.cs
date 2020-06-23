using System.Collections.Generic;
using Hiroshima.DbData.Models;

namespace Hiroshima.DirectContracts.Models
{
    public class AvailableAccommodation
    {
        public AccommodationWithLocation AccommodationWithLocation { get; set; }
        public List<AvailableRateSet> AvailableRateSets { get; set; }
    }
}