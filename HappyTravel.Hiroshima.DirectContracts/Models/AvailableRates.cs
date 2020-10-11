using System.Collections.Generic;
namespace HappyTravel.Hiroshima.DirectContracts.Models
{
    public class AvailableRates
    {
        public List<RateDetails> Rates { get; set; } = new List<RateDetails>();
    }
}