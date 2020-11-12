using System.Text.Json;

namespace HappyTravel.Hiroshima.Common.Models.Accommodations
{
    public class Amenity
    {
        public int Id { get; set; }
        public JsonDocument Name { get; set; }
    }
}
