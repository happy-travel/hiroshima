using System.Text.Json;
using HappyTravel.EdoContracts.Accommodations.Enums;
using HappyTravel.Money.Enums;

namespace HappyTravel.Hiroshima.Data.Models.Rooms
{
    public class RoomRate
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public decimal Price { get; set; }
        public int SeasonId { get; set; }
        public Currencies Currency { get; set; }
        public BoardBasisTypes BoardBasis { get; set; }
        public string MealPlan { get; set; }
        public JsonDocument Details { get; set; }
    }
}
