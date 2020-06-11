using System;

namespace Hiroshima.DbData.Models.Rooms
{
    public class RoomRateData
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CurrencyCode { get; set; }
        public string BoardBasis { get; set; }
        public string MealPlan { get; set; }
    }
}
