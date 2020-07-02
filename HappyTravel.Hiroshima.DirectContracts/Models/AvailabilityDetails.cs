using System.Collections.Generic;
using HappyTravel.Hiroshima.DbData.Models;

namespace HappyTravel.Hiroshima.DirectContracts.Models
{
    public class AvailabilityDetails
    {
        public AccommodationDetails AccommodationDetails { get; set; }
        public List<List<RateOffer>> AvailableRateOffers { get; set; }
    }
}