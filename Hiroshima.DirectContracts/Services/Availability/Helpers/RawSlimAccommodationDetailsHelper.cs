using System.Collections.Generic;
using System.Linq;
using HappyTravel.EdoContracts.Accommodations.Enums;
using HappyTravel.EdoContracts.Accommodations.Internals;
using HappyTravel.Geography;
using Hiroshima.Common.Models.Enums;
using Hiroshima.Common.Utils.Languages;
using Hiroshima.DirectContracts.Models.RawAvailiability;
using FeatureInfo = HappyTravel.EdoContracts.Accommodations.Internals.FeatureInfo;
using FeatureTypes = HappyTravel.EdoContracts.Accommodations.Enums.FeatureTypes;
using Picture = HappyTravel.EdoContracts.Accommodations.Internals.Picture;
using PropertyTypes = HappyTravel.EdoContracts.Accommodations.Enums.PropertyTypes;
using TextualDescription = HappyTravel.EdoContracts.Accommodations.Internals.TextualDescription;
using TextualDescriptionTypes = HappyTravel.EdoContracts.Accommodations.Enums.TextualDescriptionTypes;

namespace Hiroshima.DirectContracts.Services.Availability.Helpers
{
    public static class RawSlimAccommodationDetailsHelper
    {
        public static SlimAccommodationDetails CreateSlimAccommodationDetails(RawAvailabilityData rawAvailabilityData, Language language)
        {
            var accommodationId = rawAvailabilityData.Accommodation.Id.ToString();

            var accommodationAmenities =
                GetAccommodationAmenities(rawAvailabilityData, language);

            var featureInfo = GetFeatureInfo(rawAvailabilityData);

            var additionalInfo = GetAdditionalInfo(rawAvailabilityData, language);

            var name = rawAvailabilityData.Accommodation.Name.TryGetValue(language);

            var locationInfo = GetLocationInfo(rawAvailabilityData, language);

            var picture = GetPicture(rawAvailabilityData, language);

            var accommodationRating = (AccommodationRatings)rawAvailabilityData.Accommodation.Rating;

            var roomAmenities = GetRoomAmenities(rawAvailabilityData, language);

            var textualDescription = CreateTextualDescription(rawAvailabilityData, language);

            PropertyTypes propertyType = (PropertyTypes)rawAvailabilityData.Accommodation.PropertyType;


            return new SlimAccommodationDetails(
                accommodationId,
                accommodationAmenities,
                additionalInfo,
                featureInfo,
                locationInfo,
                name,
                picture,
                accommodationRating,
                roomAmenities,
                textualDescription,
                propertyType);
        }


        private static List<string> GetRoomAmenities(RawAvailabilityData rawAvailabilityData, Language language)
        {
            return rawAvailabilityData.Room.Amenities == null ||
                   !rawAvailabilityData.Room.Amenities.Any()
                ? EmptyStringList
                : rawAvailabilityData.Room.Amenities.Select(i => i.TryGetValue(language)).ToList();
        }


        private static Dictionary<string, string> GetAdditionalInfo(RawAvailabilityData rawAvailabilityData, Language language)
        {
            return rawAvailabilityData.Accommodation.AdditionalInfo == null ||
                   !rawAvailabilityData.Accommodation.AdditionalInfo.Any()
                ? EmptyStringDictionary
                : rawAvailabilityData.Accommodation.AdditionalInfo.ToDictionary(k => k.Key,
                    v => v.Value.TryGetValue(language));
        }


        private static List<string> GetAccommodationAmenities(RawAvailabilityData rawAvailabilityData, Language language)
        {
            return rawAvailabilityData.Accommodation.Amenities == null ||
                !rawAvailabilityData.Accommodation.Amenities.Any()
                ? EmptyStringList
                : rawAvailabilityData.Accommodation.Amenities.Select(i => i.TryGetValue(language)).ToList();
        }


        private static TextualDescription CreateTextualDescription(RawAvailabilityData rawAvailabilityData, Language language)
        {
            return rawAvailabilityData.Accommodation.TextualDescription == null
                ? default
             : new TextualDescription(
                (TextualDescriptionTypes)rawAvailabilityData.Accommodation.TextualDescription.Type,
                rawAvailabilityData.Accommodation.TextualDescription.Description.TryGetValue(language));
        }


        private static Picture GetPicture(RawAvailabilityData rawAvailabilityData, Language language)
        {
            return rawAvailabilityData.Accommodation.Picture == null
                ? default
                : new Picture(rawAvailabilityData.Accommodation.Picture.Source,
                rawAvailabilityData.Accommodation.Picture.Caption.TryGetValue(language));

        }


        private static SlimLocationInfo GetLocationInfo(RawAvailabilityData rawAvailabilityData, Language language) =>
                new SlimLocationInfo(rawAvailabilityData.Location.Address.TryGetValue(language),
                    rawAvailabilityData.Country.Name.TryGetValue(language),
                    rawAvailabilityData.Locality.Name.TryGetValue(language),
                    string.Empty,
                    new GeoPoint(rawAvailabilityData.Location.Coordinates.X, rawAvailabilityData.Location.Coordinates.Y));


        private static List<FeatureInfo> GetFeatureInfo(RawAvailabilityData rawAvailabilityData)
        {
            return rawAvailabilityData.Accommodation.Features?.Select(i => new FeatureInfo((FeatureTypes)i.Type, i.IsValueRequired)).ToList();
        }


        private static readonly List<string> EmptyStringList = new List<string>(0);
        private static readonly Dictionary<string, string> EmptyStringDictionary = new Dictionary<string, string>(0);
    }
}
