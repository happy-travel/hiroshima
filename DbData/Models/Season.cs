using System;
using System.Collections.Generic;

namespace Hiroshima.DbData.Models
{
    public class Season
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Accommodation Accommodation { get; set; }
        public int AccommodationId { get; set; }
        public IEnumerable<Rate> Rates { get; set; }
    }
}
