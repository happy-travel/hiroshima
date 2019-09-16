using System;
using System.Collections.Generic;
using System.Text;

namespace Hiroshima.DbData.Models
{
    public class Room
    {
        public int Id { get; set; }
        public int AccommodationId { get; set; }
        public Accommodation Accommodation { get; set; }
        public MultiLanguage<string> Name { get; set; }
        public MultiLanguage<string> Description { get; set; }
        public  Dictionary<string, MultiLanguage<string>> Amenities { get; set; }
        public IEnumerable<Rate> Rates { get; set; }
        public IEnumerable<StopSaleDate> StopSaleDates { get; set; }
        public IEnumerable<PermittedOccupancy> PermittedOccupancies{ get; set; }
    }
}
