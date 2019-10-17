using System;
using System.Collections.Generic;
using System.Text;
using Hiroshima.Common.Models;
using Hiroshima.DbData.Models;
using Hiroshima.DbData.Models.Accommodation;
using Hiroshima.DbData.Models.Common;
using Hiroshima.DbData.Models.Rooms;
using NetTopologySuite.Geometries;
using PermittedOccupancy = Hiroshima.DbData.Models.Rooms.PermittedOccupancy;

namespace Hiroshima.DirectContracts.Models.Internal
{
    public class RawAgreementData
    {
        public Point Coordinates { get; set;}
        public int AccommodationId { get; set;}
        public MultiLanguage<string> AccommodationName { get; set; }
        public MultiLanguage<string> AccommodationDescription { get; set; }
        public MultiLanguage<string> Locality { get; set; }
        public MultiLanguage<string> Country { get; set; }
        public MultiLanguage<string> Address { get; set; }
        public List<MultiLanguage<string>> AccommodationAmenities { get; set; }
        public int RateId { get; set; }
        public int RoomId { get; set; }
        public MultiLanguage<string> RoomName { get; set; }
        public decimal Price { get; set; }
        public Season Season { get; set; }
        public Dictionary<string, MultiLanguage<string>> RoomAmentities { get; set; }
        public List<Picture> Pictures { get; set; }
        public List<TextualDescription> TextualDescriptions { get; set; }
        public Contacts Contacts { get; set; }
        public PermittedOccupancy PermittedOccupancy { get; set; }
    }
}
