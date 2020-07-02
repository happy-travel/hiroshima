using System.Text.Json;

namespace HappyTravel.Hiroshima.DbData.Models.Location
{
    public class Country
    {
        public string Code { get; set; }
        public JsonDocument Name { get; set; }
    }
}