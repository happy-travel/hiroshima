using System;

namespace Hiroshima.DirectContracts.Models
{
    public class SeasonPrice
    {
        public string SeasonName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Price { get; set; }
    }
}
