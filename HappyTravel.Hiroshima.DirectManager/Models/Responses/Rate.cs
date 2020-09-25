using HappyTravel.EdoContracts.Accommodations.Enums;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Money.Enums;

namespace HappyTravel.Hiroshima.DirectManager.Models.Responses
{
    public readonly struct Rate
    {
        public Rate(int id, int roomId, int seasonId, decimal price, Currencies currency, BoardBasisTypes boardBasisType, string mealPlan, RoomTypes roomType, MultiLanguage<string> details)
        {
            Id = id;
            RoomId = roomId;
            SeasonId = seasonId;
            Price = price;
            Currency = currency;
            BoardBasisType = boardBasisType;
            MealPlan = mealPlan;
            RoomType = roomType;
            Details = details;
        }


        public int Id { get; }
        public int RoomId { get; }
        public int SeasonId { get; }
        public decimal Price { get; }
        public Currencies Currency { get; }
        public BoardBasisTypes BoardBasisType { get; }
        public string MealPlan { get; }
        public MultiLanguage<string> Details { get; }
        public RoomTypes RoomType { get; }
    }
}