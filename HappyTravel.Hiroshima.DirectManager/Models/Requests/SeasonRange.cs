using System;

namespace HappyTravel.Hiroshima.DirectManager.Models.Requests
{
    public class SeasonRange
    {
        public int SeasonId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}