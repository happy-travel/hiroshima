using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace HappyTravel.Hiroshima.Common.Infrastructure.Extensions
{
    public static class JsonDocumentExtensions
    {
        public static string GetFirstValue(this JsonDocument jsonDocument)
        {
            var firstJsonElement = GetFirst(jsonDocument);
        
            return firstJsonElement.GetString();
        }

        
        public static TResult GetFirstValue<TResult>(this JsonDocument jsonDocument)
        {
            var firstJsonElement = GetFirst(jsonDocument);
            
            return JsonSerializer.Deserialize<TResult>(firstJsonElement.ToString(), SerializeOptions);
        }
        
        
        
        
        private static JsonElement GetFirst(this JsonDocument jsonDocument)
        {
            var objectEnumerator = jsonDocument.RootElement.EnumerateObject() as IEnumerable<JsonProperty>;
            
            return objectEnumerator.FirstOrDefault().Value;
        }

        
        
        private static readonly JsonSerializerOptions SerializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
    }
}