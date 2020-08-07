using System.Collections.Generic;
using System.Text.Json;

namespace HappyTravel.Hiroshima.Common.Infrastructure.Utilities
{
    public static class JsonDocumentUtilities
    {
        public static JsonDocument CreateJDocument<T>(T value, JsonSerializerOptions serializerOptions = default)
        {
            serializerOptions ??= SerializeOptions;

            string serialized;
            if (EqualityComparer<T>.Default.Equals(value, default(T)))
            {
                var type = typeof(T);
                serialized = type.IsGenericType && type.GetGenericTypeDefinition() == typeof (IEnumerable<>)? "[]": "{}";
            }
            else
            {
                serialized = JsonSerializer.Serialize(value, serializerOptions);
            }
                
            return JsonDocument.Parse(serialized);
        }


        private static readonly JsonSerializerOptions SerializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
    }
}