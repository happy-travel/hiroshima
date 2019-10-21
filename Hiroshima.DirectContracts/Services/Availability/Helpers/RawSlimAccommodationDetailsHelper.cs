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
        public static SlimAccommodationDetails CreateSlimAccommodationDetails(RawAvailability rawAvailability, Language language)
        {
            var accommodationId = rawAvailability.Accommodation.Id.ToString();

            var accommodationAmenities =
                GetAccommodationAmenities(rawAvailability, language);

            var featureInfo = GetFeatureInfo(rawAvailability);

            var additionalInfo = GetAdditionalInfo(rawAvailability, language);

            var name = rawAvailability.Accommodation.Name.TryGetValue(language);

            var locationInfo = GetLocationInfo(rawAvailability, language);

            var picture = GetPicture(rawAvailability, language);

            var accommodationRating = (AccommodationRatings)rawAvailability.Accommodation.Rating;

            var roomAmenities = GetRoomAmenities(rawAvailability, language);

            var textualDescription = CreateTextualDescription(rawAvailability, language);

            PropertyTypes propertyType = (PropertyTypes)rawAvailability.Accommodation.PropertyType;


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


        private static List<string> GetRoomAmenities(RawAvailability rawAvailability, Language language)
        {
            return rawAvailability.Room.Amenities == null ||
                   !rawAvailability.Room.Amenities.Any()
                ? EmptyStringList
                : rawAvailability.Room.Amenities.Select(i => i.TryGetValue(language)).ToList();
        }


        private static Dictionary<string, string> GetAdditionalInfo(RawAvailability rawAvailability, Language language)
        {
            return rawAvailability.Accommodation.AdditionalInfo == null ||
                   !rawAvailability.Accommodation.AdditionalInfo.Any()
                ? EmptyStringDictionary
                : rawAvailability.Accommodation.AdditionalInfo.ToDictionary(k => k.Key,
                    v => v.Value.TryGetValue(language));
        }


        private static List<string> GetAccommodationAmenities(RawAvailability rawAvailability, Language language)
        {
            return rawAvailability.Accommodation.Amenities == null ||
                !rawAvailability.Accommodation.Amenities.Any()
                ? EmptyStringList
                : rawAvailability.Accommodation.Amenities.Select(i => i.TryGetValue(language)).ToList();
        }


        private static TextualDescription CreateTextualDescription(RawAvailability rawAvailability, Language language)
        {
            return rawAvailability.Accommodation.TextualDescription == null
                ? default
             : new TextualDescription(
                (TextualDescriptionTypes)rawAvailability.Accommodation.TextualDescription.Type,
                rawAvailability.Accommodation.TextualDescription.Description.TryGetValue(language));
        }


        private static Picture GetPicture(RawAvailability rawAvailability, Language language)
        {
            return rawAvailability.Accommodation.Picture == null
                ? default
                : new Picture(rawAvailability.Accommodation.Picture.Source,
                rawAvailability.Accommodation.Picture.Caption.TryGetValue(language));

        }


        private static SlimLocationInfo GetLocationInfo(RawAvailability rawAvailability, Language language) =>
                new SlimLocationInfo(rawAvailability.Location.Address.TryGetValue(language),
                    rawAvailability.Country.Name.TryGetValue(language),
                    rawAvailability.Locality.Name.TryGetValue(language),
                    string.Empty,
                    new GeoPoint(rawAvailability.Location.Coordinates.X, rawAvailability.Location.Coordinates.Y));


        private static List<FeatureInfo> GetFeatureInfo(RawAvailability rawAvailability)
        {
            return rawAvailability.Accommodation.FeatureInfo?.Select(i => new FeatureInfo((FeatureTypes)i.Type, i.IsValueRequired)).ToList();
        }


        private static readonly List<string> EmptyStringList = new List<string>(0);
        private static readonly Dictionary<string, string> EmptyStringDictionary = new Dictionary<string, string>(0);
    }
}
