using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Common.Models.Accommodations;

namespace HappyTravel.Hiroshima.Data.Models
{
    public class ContractAccommodationRelation
    {
        public int Id { get; set; } 
        public int ContractId { get; set; }
        public int AccommodationId { get; set; }
        public Contract Contract { get; set; }
        public Accommodation Accommodation { get; set; }
    }
}