using System.Collections.Generic;

namespace HappyTravel.Hiroshima.Common.Models.Seasons
{
    public class Season
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        
        public int ContractId { get; set; }

        public List<SeasonRange> SeasonRanges { get; set; } = new List<SeasonRange>();
        
        public Contract Contract { get; set; }
    }
}