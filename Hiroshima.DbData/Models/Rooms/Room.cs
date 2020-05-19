using System.Collections.Generic;
using Hiroshima.Common.Models;
using Hiroshima.DbData.Models.Rooms.Occupancy;

namespace Hiroshima.DbData.Models.Rooms
{
    public class Room
    {
        public int Id { get; set; }
        public int AccommodationId { get; set; }
        public MultiLanguage<string> Name { get; set; }
        public MultiLanguage<string> Description { get; set; }
        public MultiLanguage<List<string>> Amenities { get; set; }
        public PermittedOccupancies PermittedOccupancies { get; set; } 
    }
}
