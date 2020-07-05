using System.Text.Json;

namespace HappyTravel.Hiroshima.Common.Infrastructure.Utilities
{
    public static class JsonDocumentUtilities
    {
        public static JsonDocument CreateJDocument<T>(T value, JsonSerializerOptions serializerOptions = default)
        {
            serializerOptions ??= SerializeOptions;
            return JsonDocument.Parse(JsonSerializer.Serialize(value, serializerOptions));
        }


        private static readonly JsonSerializerOptions SerializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
    }
}