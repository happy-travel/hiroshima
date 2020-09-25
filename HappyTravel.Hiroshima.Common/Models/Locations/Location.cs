using System.Text.Json;

namespace HappyTravel.Hiroshima.Common.Models.Locations
{
    public class Location
    {
        public int Id { get; set; }
        
        public JsonDocument Locality { get; set; }
        
        public JsonDocument Zone { get; set; }
        
        public string CountryCode { get; set; }

        public Country Country { get; set; }
    }
}
