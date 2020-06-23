using Hiroshima.DbData.Models.Location;

namespace Hiroshima.DbData.Models
{
    public class AccommodationWithLocation
    {
        public DbData.Models.Accommodation.Accommodation Accommodation { get; set; }
        public Location.Location Location { get; set; }
        public Country Country { get; set; }
    }
}