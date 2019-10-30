using System;

namespace Hiroshima.DirectContracts.Models.Internal
{
    public class SeasonDailyPrice
    {
        public string SeasonName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
