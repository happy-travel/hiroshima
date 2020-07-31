using System;
using System.Text.Json;
using HappyTravel.EdoContracts.Accommodations.Enums;
using HappyTravel.Hiroshima.Data.Models;
using HappyTravel.Hiroshima.Data.Models.Rooms;
using HappyTravel.Money.Enums;

namespace HappyTravel.Hiroshima.DirectContracts.Models
{
    public class RateDetails
    {
        public int RateId { get; set; }
        public int RoomId { get; set; }
        public decimal Price { get; set; }
        public int SeasonId { get; set; }
        public Currencies Currency { get; set; }
        public BoardBasisTypes BoardBasis { get; set; }
        public string MealPlan { get; set; }
        public JsonDocument Details { get; set; }
        public string SeasonName { get; set; }
        public DateTime SeasonStartDate { get; set; }
        public DateTime SeasonEndDate { get; set; }
        public int SeasonContractId { get; set; }
    }
}