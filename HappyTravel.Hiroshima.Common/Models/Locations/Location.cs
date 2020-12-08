using System.Text.Json;

namespace HappyTravel.Hiroshima.Common.Models.Locations
{
    public class Location
    {
        public int Id { get; set; }
        
        public MultiLanguage<string> Locality { get; set; }
        
        public MultiLanguage<string> Zone { get; set; }

        public string CountryCode { get; set; } = string.Empty;

        public Country Country { get; set; }
    }
}
