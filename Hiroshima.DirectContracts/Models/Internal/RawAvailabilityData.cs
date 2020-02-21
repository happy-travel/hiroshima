using Hiroshima.DbData.Models.Accommodation;
using Hiroshima.DbData.Models.Booking;
using Hiroshima.DbData.Models.Location;
using Hiroshima.DbData.Models.Rates;
using Hiroshima.DbData.Models.Rooms;

namespace Hiroshima.DirectContracts.Models.RawAvailiability
{
    public class RawAvailabilityData
    {
        public Location Location { get; set; }
        public Locality Locality { get; set; }
        public Country Country { get; set; }
        public Accommodation Accommodation { get; set; }
        public Room Room { get; set; }
        public Season Season { get; set; }
        public ContractedRate ContractedRate { get; set; }
        public RoomDetails RoomDetails { get; set; }
        public DiscountRate DiscountRate { get; set; }
        public CancelationPolicy CancelationPolicy { get; set; }
    }
}