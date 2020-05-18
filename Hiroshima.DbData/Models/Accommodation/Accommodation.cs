using System.Collections.Generic;
using HappyTravel.EdoContracts.Accommodations.Enums;
using Hiroshima.Common.Models;
using Hiroshima.Common.Models.Enums;
using NetTopologySuite.Geometries;

namespace Hiroshima.DbData.Models.Accommodation
{
    public class Accommodation
    {
        public int Id { get; set; }
        public MultiLanguage<string> Name { get; set; }
        public MultiLanguage<string> Address { get; set; }
        public List<TextualDescription> TextualDescription { get; set; }
        public Point Coordinates { get; set; }
        public AccommodationRating Rating { get; set; }
        public string CheckInTime { get; set; }
        public string CheckOutTime { get; set; }
        public List<Picture> Pictures { get; set; }
        public Contacts Contacts { get; set; }
        public PropertyTypes PropertyType { get; set; }
        public List<MultiLanguage<string>> AccommodationAmenities { get; set; }
        public List<string> RoomAmenities { get; set; }
        public Dictionary<string, MultiLanguage<string>> AdditionalInfo { get; set; }
        public int LocationId { get; set; }
    }
}
