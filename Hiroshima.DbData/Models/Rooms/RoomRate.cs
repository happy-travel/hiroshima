using System;

namespace Hiroshima.DbData.Models.Rooms
{
    public class RoomRate
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public decimal Price { get; set; }
        public DateTime StartsFromDate { get; set; }
        public DateTime EndsToDate { get; set; }
        public string CurrencyCode { get; set; }
        public string BoardBasis { get; set; }
        public string MealPlan { get; set; }
    }
}
