using HappyTravel.Hiroshima.Common.Models.Accommodations;
using HappyTravel.Hiroshima.Common.Models.Locations;

namespace HappyTravel.Hiroshima.DirectContracts.Models
{
    public class AccommodationDetails
    {
        public Accommodation Accommodation { get; set; }
        public Location Location { get; set; }
        public Country Country { get; set; }
    }
}