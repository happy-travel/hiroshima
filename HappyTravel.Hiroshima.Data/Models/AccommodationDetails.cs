using HappyTravel.Hiroshima.Data.Models.Accommodations;
using HappyTravel.Hiroshima.Data.Models.Location;

namespace HappyTravel.Hiroshima.Data.Models
{
    public class AccommodationDetails
    {
        public Accommodation Accommodation { get; set; }
        public Location.Location Location { get; set; }
        public Country Country { get; set; }
    }
}