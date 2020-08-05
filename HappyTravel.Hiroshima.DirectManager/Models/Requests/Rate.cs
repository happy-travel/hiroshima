using System.ComponentModel.DataAnnotations;
using HappyTravel.EdoContracts.Accommodations.Enums;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Money.Enums;

namespace HappyTravel.Hiroshima.DirectManager.Models.Requests
{
    public class Rate
    {
        [Required]
        public int RoomId { get; set; }
        
        [Required]
        public int SeasonId { get; set; }
        
        [Required]
        public decimal Price { get; set; }
        
        [Required]
        public Currencies Currency { get; set; }
        
        [Required]
        public BoardBasisTypes BoardBasis { get; set; }
        
        [Required]
        public string MealPlan { get; set; }
        
        public MultiLanguage<string> Details { get; set; }
    }
}