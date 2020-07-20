using System.Text.Json;

namespace HappyTravel.Hiroshima.Data.Models.Rooms
{
    public class RoomRate
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public decimal Price { get; set; }
        public int SeasonId { get; set; }
        public string CurrencyCode { get; set; }
        public string BoardBasis { get; set; }
        public string MealPlan { get; set; }
        public JsonDocument Details { get; set; }
    }
}
