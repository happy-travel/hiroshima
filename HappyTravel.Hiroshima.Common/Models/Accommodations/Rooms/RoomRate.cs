using System.Text.Json;
using HappyTravel.EdoContracts.Accommodations.Enums;
using HappyTravel.Hiroshima.Common.Models.Seasons;
using HappyTravel.Money.Enums;

namespace HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms
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
        
        public Room Room { get; set; }
        
        public Season Season { get; set; }
    }
}
