using System.Collections.Generic;
using Hiroshima.DbData.Models;

namespace Hiroshima.DirectContracts.Models
{
    public class AvailabilityDetails
    {
        public AccommodationDetails AccommodationDetails { get; set; }
        public List<List<RateOffer>> AvailableRateOffers { get; set; }
    }
}