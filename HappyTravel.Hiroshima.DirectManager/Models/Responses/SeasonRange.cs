using System;

namespace HappyTravel.Hiroshima.DirectManager.Models.Responses
{
    public readonly struct SeasonRange
    {
        public SeasonRange(int id, int seasonId, DateTime startDate, DateTime endDate)
        {
            Id = id;
            SeasonId = seasonId;
            StartDate = startDate;
            EndDate = endDate;
        }
        
        
        public int Id { get; }
        public int SeasonId { get; }
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }    
    }
}