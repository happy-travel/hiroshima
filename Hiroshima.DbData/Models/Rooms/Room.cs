using System.Collections.Generic;
using Hiroshima.Common.Models;
using Hiroshima.DbData.Models.Rates;

namespace Hiroshima.DbData.Models.Rooms
{
    public class Room
    {
        public int Id { get; set; }
        public int AccommodationId { get; set; }
        public Accommodation.Accommodation Accommodation { get; set; }
        public MultiLanguage<string> Name { get; set; }
        public MultiLanguage<string> Description { get; set; }
        public List<MultiLanguage<string>> Amenities { get; set; }
        public ICollection<ContractedRate> ContractRates { get; set; }
        public ICollection<StopSaleDate> StopSaleDates { get; set; }
        public ICollection<RoomDetails> RoomDetails { get; set; }
        public ICollection<DiscountRate> DiscountRates { get; set; }
    }
}