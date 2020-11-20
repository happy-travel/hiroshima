using System.Linq;
using HappyTravel.EdoContracts.Accommodations.Internals;
using HappyTravel.Geography;
using HappyTravel.Hiroshima.Common.Infrastructure.Extensions;
using HappyTravel.Hiroshima.Common.Infrastructure.Utilities;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Common.Models.Accommodations;
using HappyTravel.Hiroshima.WebApi.Controllers.DirectManager;
using PropertyTypes = HappyTravel.EdoContracts.Accommodations.Enums.PropertyTypes;

namespace HappyTravel.Hiroshima.WebApi.Services.AvailabilitySearch
{
    public class AccommodationResponseService : IAccommodationResponseService
    {
        public SlimAccommodation Create(Accommodation accommodation, string languageCode)
        {
            var id = accommodation.Id.ToString();
            var location = GetSlimLocation(accommodation, languageCode);
            var name = GetName(accommodation, languageCode);
            var firstImage = GetFirstImage(accommodation, languageCode);
            var rating = AccommodationRatingConverter.Convert(accommodation.Rating);
            var propertyType = (PropertyTypes) accommodation.PropertyType;
            
            return new SlimAccommodation(id, location, name, firstImage, rating, propertyType);
        }

        
        private string GetName(Accommodation accommodation, string languageCode)
        {
            accommodation.Name.GetValue<MultiLanguage<string>>().TryGetValueOrDefault(languageCode, out var name);
            
            return name;
        }


        private SlimLocationInfo GetSlimLocation(Accommodation accommodation, string languageCode)
        {
            accommodation.Address.GetValue<MultiLanguage<string>>().TryGetValueOrDefault(languageCode, out var address);
            accommodation.Location.Country.Name.GetValue<MultiLanguage<string>>().TryGetValueOrDefault(languageCode, out var country);
            var countryCode = accommodation.Location.CountryCode;
            accommodation.Location.Locality.GetValue<MultiLanguage<string>>().TryGetValueOrDefault(languageCode, out var locality);
            accommodation.Location.Zone.GetValue<MultiLanguage<string>>().TryGetValueOrDefault(languageCode, out var zone);
            var coordinates = new GeoPoint(accommodation.Coordinates);
            
            return new SlimLocationInfo(address, country, countryCode, locality, zone, coordinates);
        }


        private ImageInfo GetFirstImage(Accommodation accommodation, string languageCode)
        {
            if (!accommodation.Images.Any()) 
               return new ImageInfo();

            //var firstImage = accommodation.Images.OrderBy(image => image.Position).First();
            var firstImage = accommodation.Images[0].RootElement;
            //firstImage.Description.GetValue<MultiLanguage<string>>().TryGetValueOrDefault(languageCode, out var caption);

            //return new ImageInfo(firstImage.MainImage.Url, caption); 
            return new ImageInfo();
        }
    }
}