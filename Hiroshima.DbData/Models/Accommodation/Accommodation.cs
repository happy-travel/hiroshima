using System.Collections.Generic;
using Hiroshima.Common.Models;
using Hiroshima.DbData.Models.Common;
using Hiroshima.DbData.Models.Enums;
using Hiroshima.DbData.Models.Rooms;

namespace Hiroshima.DbData.Models.Accommodation
{
    public class Accommodation
    {
        public int Id { get; set; }
        public Location.Location Location { get; set; }
        public MultiLanguage<string> Name { get; set; }
        public MultiLanguage<string> Description { get; set; }
        public Contacts Contacts { get; set; }
        public List<Picture> Pictures { get; set; }
        public HotelRating Rating { get; set; }
        public List<TextualDescription> TextualDescriptions { get; set; }
        public Schedule Schedule { get; set; }
        public string Category { get; set; }
        public PropertyType PropertyType { get; set; }
        public List<MultiLanguage<string>> Amenities { get; set; }
        public Dictionary<string, MultiLanguage<string>> AdditionalInfo { get; set; }
        public IEnumerable<Room> Rooms { get; set; }
        public IEnumerable<Season> Seasons { get; set; } 
    }
}
