using System;

namespace HappyTravel.Hiroshima.Data.Models.Seasons
{
    public class SeasonRange
    {
        public int Id { get; set; }
        public int SeasonId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}