using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Hiroshima.DbData.Models.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum LocationDescriptionCodes
    {
        Unspecified = 0,
        CityCenter = 1,
        Airport = 2,
        RailwayStation = 3,
        Port = 4,
        SeaOrBeach = 5,
        OpenCountry = 6,
        Mountains = 7,
        Peripherals = 8,
        CloseToCityCentre = 9,
        City = 10,
        Island = 11,
        Ranch = 12,
        Boutique = 13,
        WaterFront = 14,
        OceanFront = 15,
        Desert = 16,
    }
}
