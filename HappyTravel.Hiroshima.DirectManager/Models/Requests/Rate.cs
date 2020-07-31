using HappyTravel.EdoContracts.Accommodations.Enums;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Money.Enums;

namespace HappyTravel.Hiroshima.DirectManager.Models.Requests
{
    public class Rate
    {
        public int RoomId { get; set; }
        public int SeasonId { get; set; }
        public decimal Price { get; set; }
        public Currencies Currency { get; set; }
        public BoardBasisTypes BoardBasis { get; set; }
        public string MealPlan { get; set; }
        public MultiLanguage<string> Details { get; set; }
    }
}