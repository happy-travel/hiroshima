using System.Collections.Generic;
using System.Text.Json;
using HappyTravel.EdoContracts.Accommodations.Enums;
using Hiroshima.Common.Models;
using Hiroshima.Common.Models.Enums;
using NetTopologySuite.Geometries;

namespace Hiroshima.DbData.Models.Accommodation
{
    public class Accommodation: BaseModel
    {
        public int Id { get; set; }
        public JsonDocument Name { get; set; }
        public JsonDocument Address { get; set; }
        public JsonDocument TextualDescription { get; set; }
        public Point Coordinates { get; set; }
        public AccommodationRating Rating { get; set; }
        public string CheckInTime { get; set; }
        public string CheckOutTime { get; set; }
        public JsonDocument Pictures { get; set; }
        public Contacts Contacts { get; set; }
        public PropertyTypes PropertyType { get; set; }
        public JsonDocument AccommodationAmenities { get; set; }
        public JsonDocument AdditionalInfo { get; set; }
        public OccupancyDefinition OccupancyDefinition { get; set; }
        public int LocationId { get; set; }

        
        public string GetNameFromFirstLanguage()
            => GetStringFromFirstLanguage(Name);
        
        
        public void SetName(MultiLanguage<string> name)
            => Name = CreateJDocument(name);            
        
        
        public string GetAddressFromFirstLanguage()
            => GetStringFromFirstLanguage(Address);

        
        public void SetAddress(MultiLanguage<string> address)
            => Address = CreateJDocument(address);     
        

        public TextualDescription GetTextualDescriptionFromFirstLanguage()
            => GetValueFromFirstLanguage<TextualDescription>(TextualDescription);
        
        
        public void SetTextualDescription(MultiLanguage<TextualDescription> textualDescription)
            => TextualDescription = CreateJDocument(textualDescription);
        
        
        public List<Picture> GetPicturesFromFirstLanguage()
            => GetValueFromFirstLanguage<List<Picture>>(Pictures);

        
        public void SetPictures(MultiLanguage<List<Picture>> pictures)
            => Pictures = CreateJDocument(pictures);
        
        
        public List<string> GetAccommodationAmenitiesFromFirstLanguage()
            => GetValueFromFirstLanguage<List<string>>(AccommodationAmenities);

        
        public void SetAccommodationAmenities(MultiLanguage<List<string>> accommodationAmenities)
            => AccommodationAmenities = CreateJDocument(accommodationAmenities);
        
        
        public Dictionary<string, string> GetAdditionalInfoFromFirstLanguage()
            => GetValueFromFirstLanguage<Dictionary<string, string>>(AdditionalInfo);
        

        public void SetAdditionalInfo(MultiLanguage<Dictionary<string, string>> additionalInfo)
            => AdditionalInfo = CreateJDocument(additionalInfo);
    }
}
