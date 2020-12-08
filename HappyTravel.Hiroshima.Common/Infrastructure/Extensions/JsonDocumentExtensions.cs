using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Newtonsoft.Json;

namespace HappyTravel.Hiroshima.Common.Infrastructure.Extensions
{
    public static class JsonDocumentExtensions
    {
        public static string GetFirstValue(this JsonDocument jsonDocument)
        {
            if (jsonDocument is null)
                return string.Empty;
            
            var firstJsonElement = GetFirst(jsonDocument);

            var result = firstJsonElement.GetString();
            
            return result ?? string.Empty ;
        }


        public static TResult GetValue<TResult>(this JsonDocument jsonDocument)
        {
            var json = jsonDocument.RootElement.ToString();
            if (string.IsNullOrEmpty(json))
                return default;

            return JsonConvert.DeserializeObject<TResult>(json);
        }


        public static bool IsNotEmpty(this JsonDocument jsonDocument)
            => jsonDocument.RootElement.ToString() != "{}";
        
        
        public static TResult GetFirstValue<TResult>(this JsonDocument jsonDocument)
        {
            if (jsonDocument is null)
                return default;
            
            var firstJsonElement = GetFirst(jsonDocument);
            
            return JsonConvert.DeserializeObject<TResult>(firstJsonElement.ToString());
        }
        
        
        private static JsonElement GetFirst(this JsonDocument jsonDocument)
        {
            var objectEnumerator = jsonDocument.RootElement.EnumerateObject() as IEnumerable<JsonProperty>;
            
            return objectEnumerator.FirstOrDefault().Value;
        }
    }
}