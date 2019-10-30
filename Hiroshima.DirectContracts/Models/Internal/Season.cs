using System;
namespace Hiroshima.DirectContracts.Models.Internal
{
    public struct Season
    {
        public Season(string seasonName, DateTime startDate, DateTime endDate, decimal nightPrice)
        {
            SeasonName = seasonName;
            StartDate = startDate;
            EndDate = endDate;
            NightPrice = nightPrice;
        }


        public string SeasonName { get; }
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }
        public decimal NightPrice { get; }
    }
}
