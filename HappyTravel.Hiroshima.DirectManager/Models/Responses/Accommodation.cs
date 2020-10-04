using System;
using System.Collections.Generic;
using HappyTravel.Geography;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Common.Models.Accommodations;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.OccupancyDefinitions;
using HappyTravel.Hiroshima.Common.Models.Enums;

namespace HappyTravel.Hiroshima.DirectManager.Models.Responses
{
    public readonly struct Accommodation
    {
        public Accommodation(int id, MultiLanguage<string> name, MultiLanguage<string> address, MultiLanguage<TextualDescription> description, GeoPoint coordinates, AccommodationRating rating, string checkInTime, string checkOutTime, MultiLanguage<List<Picture>> pictures, ContactInfo contactInfo, PropertyTypes propertyType, MultiLanguage<List<string>> amenities, MultiLanguage<string> additionalInfo, OccupancyDefinition occupancyDefinition, int locationId, MultiLanguage<List<string>> leisureAndSports, Status status, RateOptions rateOptions, int? floor, int? buildYear, List<int> roomIds)
        {
            Id = id;
            Name = name;
            Address = address;
            Description = description;
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
            LocationId = locationId;
            RateOptions = rateOptions;
            Status = status;
            LeisureAndSports = leisureAndSports;
            RoomIds = roomIds;
            Floor = floor;
            BuildYear = buildYear;
        }


        public int Id { get; }
        
        public MultiLanguage<string> Name { get; }
        
        public MultiLanguage<string> Address { get; }
        
        public MultiLanguage<TextualDescription> Description { get; }
        
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
        
        public int LocationId { get; }
        
        public MultiLanguage<List<string>> LeisureAndSports { get; }
        
        public RateOptions RateOptions { get; }
        
        public int? Floor { get; }
        
        public int? BuildYear { get; }

        public Status Status { get; }

        public List<int> RoomIds { get; }
    }
}