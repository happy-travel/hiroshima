using System.Collections.Generic;
using System.Text.Json;

namespace HappyTravel.Hiroshima.Common.Models.Locations
{
    public class Country
    {
        public string Code { get; set; } = string.Empty;
        
        public JsonDocument Name { get; set; }

        public List<Location> Locations { get; set; } = new List<Location>();
    }
}