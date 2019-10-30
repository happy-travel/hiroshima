using System.Collections.Generic;
using Hiroshima.Common.Models;
using Hiroshima.Common.Models.Accommodation;
using Hiroshima.Common.Models.Enums;
using Hiroshima.DbData.Models.Rooms;

namespace Hiroshima.DbData.Models.Accommodation
{
    public class Accommodation
    {
        public int Id { get; set; }
        public Location.Location Location { get; set; }
        public MultiLanguage<string> Name { get; set; }
        public Contacts Contacts { get; set; }
        public Picture Picture { get; set; }
        public AccommodationRating Rating { get; set; }
        public TextualDescription TextualDescription { get; set; }
        public Schedule Schedule { get; set; }
        public PropertyTypes PropertyType { get; set; }
        public List<FeatureInfo> Features { get; set; }
        public List<MultiLanguage<string>> Amenities { get; set; }
        public Dictionary<string, MultiLanguage<string>> AdditionalInfo { get; set; }
        public ICollection<Room> Rooms { get; set; }
        public ICollection<Season> Seasons { get; set; } 
    }
}
