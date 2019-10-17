using System;
using System.Collections.Generic;
using Hiroshima.DbData.Models.Rates;

namespace Hiroshima.DbData.Models.Rooms
{
    public class Season
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Accommodation.Accommodation Accommodation { get; set; }
        public int AccommodationId { get; set; }
        public IEnumerable<Rate> Rates { get; set; }
    }
}
