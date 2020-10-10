using System;
using HappyTravel.Hiroshima.Common.Infrastructure.Utilities;

namespace HappyTravel.Hiroshima.Common.Models.Seasons
{
    public class SeasonRange
    {
        public int Id { get; set; }
        
        public int SeasonId { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public DateTime EndDate { get; set; }
        
        public Season Season { get; set; }
        
        
        public bool Intersects(SeasonRange range) => DateRange.AreIntersect(StartDate, EndDate, range.StartDate, range.EndDate);
    }
}