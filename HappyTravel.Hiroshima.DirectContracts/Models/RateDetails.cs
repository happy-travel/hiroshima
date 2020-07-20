using System;
using System.Text.Json;
using HappyTravel.Hiroshima.Data.Models;
using HappyTravel.Hiroshima.Data.Models.Rooms;

namespace HappyTravel.Hiroshima.DirectContracts.Models
{
    public class RateDetails
    {
        public int RateId { get; set; }
        public int RoomId { get; set; }
        public decimal Price { get; set; }
        public int SeasonId { get; set; }
        public string CurrencyCode { get; set; }
        public string BoardBasis { get; set; }
        public string MealPlan { get; set; }
        public JsonDocument Details { get; set; }
        public string SeasonName { get; set; }
        public DateTime SeasonStartDate { get; set; }
        public DateTime SeasonEndDate { get; set; }
        public int SeasonContractId { get; set; }
    }
}