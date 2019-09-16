using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Hiroshima.DbData.Models.Enums;

namespace Hiroshima.DbData.Models
{
    public class Accommodation
    {
        public int Id { get; set; }
        public Location Location { get; set; }
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
