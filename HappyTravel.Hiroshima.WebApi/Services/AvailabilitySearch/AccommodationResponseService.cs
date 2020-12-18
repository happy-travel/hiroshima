using System;
using System.Collections.Generic;
using System.Linq;
using HappyTravel.EdoContracts.Accommodations.Enums;
using HappyTravel.EdoContracts.Accommodations.Internals;
using HappyTravel.Geography;
using HappyTravel.Hiroshima.Common.Infrastructure.Utilities;
using HappyTravel.Hiroshima.Common.Models.Accommodations;
using HappyTravel.Hiroshima.DirectManager.Services;
using ContactInfo = HappyTravel.EdoContracts.Accommodations.Internals.ContactInfo;
using PropertyTypes = HappyTravel.EdoContracts.Accommodations.Enums.PropertyTypes;
using TextualDescription = HappyTravel.EdoContracts.Accommodations.Internals.TextualDescription;

namespace HappyTravel.Hiroshima.WebApi.Services.AvailabilitySearch
{
    public class AccommodationResponseService : IAccommodationResponseService
    {
        public AccommodationResponseService(IImageManagementService imageManagementService)
        {
            _imageManagementService = imageManagementService;
        }
        
        public SlimAccommodation CreateSlim(Accommodation accommodation, string languageCode)
        {
            var id = accommodation.Id.ToString();
            var location = GetSlimLocation(accommodation, languageCode);
            var name = GetName(accommodation, languageCode);
            var firstImage = GetFirstImage(accommodation, languageCode);
            var rating = AccommodationRatingConverter.Convert(accommodation.Rating);
            var propertyType = (PropertyTypes) accommodation.PropertyType;
            
            return new SlimAccommodation(id, location, name, firstImage, rating, propertyType);
        }


        public EdoContracts.Accommodations.Accommodation Create(Accommodation accommodation, string languageCode)
        {
            var id = accommodation.Id.ToString();
            var name = GetName(accommodation, languageCode);
            var amenities = GetAmenities(accommodation, languageCode);
            var additionalInfo = GetAdditionalInfo(accommodation, languageCode);
            var category = Enum.GetName(typeof(PropertyTypes), accommodation.PropertyType)!;
            var contacts = GetContactInfo(accommodation);
            var locationInfo = GetLocationInfo(accommodation, languageCode);
            var imageInfo = GetImageInfo(accommodation, languageCode);
            var rating = AccommodationRatingConverter.Convert(accommodation.Rating);
            var scheduleInfo = GetScheduleInfo(accommodation);
            var textualDescription = GetTextualDescription(accommodation, languageCode);
            var propertyType = GetPropertyType(accommodation);
            
            return new EdoContracts.Accommodations.Accommodation(id, name, amenities, additionalInfo, category, contacts, locationInfo, imageInfo, rating, scheduleInfo, textualDescription, propertyType);
        }


        private string GetName(Accommodation accommodation, string languageCode)
        {
            accommodation.Name.TryGetValueOrDefault(languageCode, out var name);
            
            return name;
        }


        private List<string> GetAmenities(Accommodation accommodation, string languageCode)
        {
            accommodation.AccommodationAmenities.TryGetValueOrDefault(languageCode, out var amenities);
            
            return amenities;
        }


        private Dictionary<string, string> GetAdditionalInfo(Accommodation accommodation, string languageCode)
        {
            accommodation.AdditionalInfo.TryGetValue(languageCode, out var additionalInfo);

            return new Dictionary<string, string>{{string.Empty, additionalInfo}};
        }


        private ContactInfo GetContactInfo(Accommodation accommodation)
        {
            var contactInfo = accommodation.ContactInfo;
            
            return new ContactInfo(contactInfo.Emails, contactInfo.Phones, contactInfo.Websites, contactInfo.Faxes);
        }


        private LocationInfo GetLocationInfo(Accommodation accommodation, string languageCode)
        {
            var location = accommodation.Location;
            var country = accommodation.Location.Country;
            
            country.Name.TryGetValueOrDefault(languageCode, out var countryName);
            location.Locality.TryGetValueOrDefault(languageCode, out var locality);
            location.Zone.TryGetValueOrDefault(languageCode, out var zone);
            accommodation.Address.TryGetValueOrDefault(languageCode, out var address);
            var locationDescriptionCodes = new LocationDescriptionCodes();
            var poiInfo = new List<PoiInfo>();
            var coordinates = new GeoPoint(accommodation.Coordinates);
            
            return new LocationInfo(location.CountryCode, 
                countryName, 
                string.Empty,
                locality, 
                string.Empty, 
                zone, 
                coordinates, 
                address,
                locationDescriptionCodes,
                poiInfo);
        }


        private List<ImageInfo> GetImageInfo(Accommodation accommodation, string languageCode) => accommodation.Images.Select(i =>
        {
            i.Description.TryGetValueOrDefault(languageCode, out var caption);
            return new ImageInfo(i.LargeImageURL, caption);
        }).ToList();


        private ScheduleInfo GetScheduleInfo(Accommodation accommodation) 
            => new ScheduleInfo(accommodation.CheckInTime, accommodation.CheckOutTime);


        private List<TextualDescription> GetTextualDescription(Accommodation accommodation, string languageCode)
        {
            accommodation.TextualDescription.TryGetValueOrDefault(languageCode, out var textualDescription);
            
            return new List<TextualDescription>{new TextualDescription(textualDescription.Type, textualDescription.Description)};
        }


        private PropertyTypes GetPropertyType(Accommodation accommodation)
            => accommodation.PropertyType switch
            {
                Common.Models.Accommodations.PropertyTypes.Apartments => PropertyTypes.Apartments,
                Common.Models.Accommodations.PropertyTypes.Hotels => PropertyTypes.Hotels,
                _ => throw new ArgumentException($"Invalid value {accommodation.PropertyType}", nameof(accommodation.PropertyType))
            };
        

        private SlimLocationInfo GetSlimLocation(Accommodation accommodation, string languageCode)
        {
            accommodation.Address.TryGetValueOrDefault(languageCode, out var address);
            accommodation.Location.Country.Name.TryGetValueOrDefault(languageCode, out var country);
            var countryCode = accommodation.Location.CountryCode;
            accommodation.Location.Locality.TryGetValueOrDefault(languageCode, out var locality);
            accommodation.Location.Zone.TryGetValueOrDefault(languageCode, out var zone);
            var coordinates = new GeoPoint(accommodation.Coordinates);
            
            return new SlimLocationInfo(address, country, countryCode, locality, zone, coordinates);
        }


        private ImageInfo GetFirstImage(Accommodation accommodation, string languageCode)
        {
            if (!accommodation.Images.Any()) 
               return new ImageInfo();

            var firstImage = accommodation.Images.First();
            
            var firstImageUrl = _imageManagementService.GetImageUrl(firstImage.LargeImageURL);
            firstImage.Description.TryGetValueOrDefault(languageCode, out var caption);

            return new ImageInfo(firstImageUrl, caption); 
        }
        
        
        private readonly IImageManagementService _imageManagementService;
    }
}