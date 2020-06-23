using System;
using System.Text.Json;
using Hiroshima.Common.Models;

namespace Hiroshima.DbData.Models.Rooms
{
    public class RoomRate: BaseModel
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CurrencyCode { get; set; }
        public string BoardBasis { get; set; }
        public string MealPlan { get; set; }
        public JsonDocument Details { get; set; }
        
        
        public string GetDetailsFromFirstLanguage()
            => GetStringFromFirstLanguage(Details);
        
        
        public void SetDetails(MultiLanguage<string> details)
            => Details = CreateJDocument(details);       
    }
}
