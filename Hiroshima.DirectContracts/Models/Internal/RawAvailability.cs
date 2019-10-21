using Hiroshima.DbData.Models.Accommodation;
using PermittedOccupancy = Hiroshima.DbData.Models.Rooms.PermittedOccupancy;
using Hiroshima.DbData.Models.Location;
using Hiroshima.DbData.Models.Rates;
using Hiroshima.DbData.Models.Rooms;
using Location = Hiroshima.DbData.Models.Location.Location;

namespace Hiroshima.DirectContracts.Models.RawAvailiability
{
    public class RawAvailability
    {
      public Location Location { get; set; }
      public Locality Locality { get; set; }
      public Country Country { get; set; }
      public Accommodation Accommodation { get; set; }
      public Room Room { get; set; }
      public Season Season { get; set; }
      public Rate Rate { get; set; }
      public PermittedOccupancy PermittedOccupancy { get; set; }
    }
}
