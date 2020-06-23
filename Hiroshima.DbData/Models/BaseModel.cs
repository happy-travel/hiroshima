using System.Linq;
using System.Text.Json;

namespace Hiroshima.DbData.Models
{
    public abstract class BaseModel
    {
        protected string GetStringFromFirstLanguage(JsonDocument jsonDocument)
        {
            var firstJsonElement = GetFirst(jsonDocument);
        
            return firstJsonElement.GetString();
        }

        
        private JsonElement GetFirst(JsonDocument jsonDocument)
        {
            var objectEnumerator = jsonDocument.RootElement.EnumerateObject();
            objectEnumerator.MoveNext();
            return objectEnumerator.Current.Value;
        }

        
        protected TResult GetValueFromFirstLanguage<TResult>(JsonDocument jsonDocument)
        {
            var firstJsonElement = GetFirst(jsonDocument);
            
            return JsonSerializer.Deserialize<TResult>(firstJsonElement.ToString(), SerializeOptions);
        }
        
        
        protected JsonDocument CreateJDocument<T>(T value)
            => JsonDocument.Parse(JsonSerializer.Serialize(value, SerializeOptions));
        
        
        private static readonly JsonSerializerOptions SerializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
    }
}