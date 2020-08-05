using System.Collections.Generic;
using HappyTravel.Geography;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Common.Models.Accommodations;
using HappyTravel.Hiroshima.Common.Models.Enums;

namespace HappyTravel.Hiroshima.DirectManager.Models.Responses
{
    public readonly struct AccommodationResponse
    {
        public AccommodationResponse(int id, MultiLanguage<string> name, MultiLanguage<string> address, MultiLanguage<TextualDescription> textualDescription, GeoPoint coordinates, AccommodationRating rating, string checkInTime, string checkOutTime, MultiLanguage<List<Picture>> pictures, ContactInfo contactInfo, PropertyTypes propertyType, MultiLanguage<List<string>> amenities, MultiLanguage<string> additionalInfo, OccupancyDefinition occupancyDefinition, List<int> roomIds)
        {
            Id = id;
            Name = name;
            Address = address;
            TextualDescription = textualDescription;
            Coordinates = coordinates;
            Rating = rating;
            CheckInTime = checkInTime;
            CheckOutTime = checkOutTime;
            Pictures = pictures;
            ContactInfo = contactInfo;
            PropertyType = propertyType;
            Amenities = amenities;
            AdditionalInfo = additionalInfo;
            OccupancyDefinition = occupancyDefinition;
            RoomIds = roomIds;
        }


        public int Id { get; }
        public MultiLanguage<string> Name { get; }
        public MultiLanguage<string> Address { get; }
        public MultiLanguage<TextualDescription> TextualDescription { get; }
        public GeoPoint Coordinates { get; }
        public AccommodationRating Rating { get; }
        public string CheckInTime { get; }
        public string CheckOutTime { get; }
        public MultiLanguage<List<Picture>> Pictures { get; }
        public ContactInfo ContactInfo { get; }
        public PropertyTypes PropertyType { get; }
        public MultiLanguage<List<string>> Amenities { get; }
        public MultiLanguage<string> AdditionalInfo { get; }
        public OccupancyDefinition OccupancyDefinition { get; }
        public List<int> RoomIds { get; }
    }
}