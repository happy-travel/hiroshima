using System.Text.Json;

namespace HappyTravel.Hiroshima.Data.Models.Location
{
    public class Country
    {
        public string Code { get; set; }
        public JsonDocument Name { get; set; }
    }
}