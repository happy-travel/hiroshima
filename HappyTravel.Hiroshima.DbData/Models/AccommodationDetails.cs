using HappyTravel.Hiroshima.DbData.Models.Location;

namespace HappyTravel.Hiroshima.DbData.Models
{
    public class AccommodationDetails
    {
        public DbData.Models.Accommodation.Accommodation Accommodation { get; set; }
        public Location.Location Location { get; set; }
        public Country Country { get; set; }
    }
}