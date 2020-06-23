using System.Collections.Generic;
using System.Text.Json;
using Hiroshima.Common.Models;
using Hiroshima.DbData.Models.Rooms.Occupancy;

namespace Hiroshima.DbData.Models.Rooms
{
    public class Room: BaseModel
    {
        public int Id { get; set; }
        public int AccommodationId { get; set; }
        public JsonDocument Name { get; set; }
        public JsonDocument Description { get; set; }
        public JsonDocument Amenities { get; set; }
        public List<OccupancyConfiguration> OccupancyConfigurations { get; set; }
        
        
        public string GetNameFromFirstLanguage()
            => GetStringFromFirstLanguage(Name);
        
        
        public void SetName(MultiLanguage<string> name)
            => Name = CreateJDocument(name);       
        
        
        public string GetDescriptionFromFirstLanguage()
            => GetStringFromFirstLanguage(Description);
        
        
        public void SetDescription(MultiLanguage<string> description)
            => Description = CreateJDocument(description);


        public List<string> GetAmenities()
            => GetValueFromFirstLanguage<List<string>>(Amenities);
        
        
        public void SetAmenities(MultiLanguage<List<string>> amenities)
            => Amenities = CreateJDocument(amenities);
    }
}
