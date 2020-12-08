using System.Collections.Generic;
using System.Text.Json;
using Newtonsoft.Json;
namespace HappyTravel.Hiroshima.Common.Infrastructure.Utilities
{
    public static class JsonDocumentUtilities
    {
        public static JsonDocument CreateJDocument<T>(T value, JsonSerializerSettings serializeSettings = null)
        {
            string serialized;
            if (EqualityComparer<T>.Default.Equals(value, default(T)))
            {
                var type = typeof(T);
                serialized = type.IsGenericType && type.GetGenericTypeDefinition() == typeof (IEnumerable<>)? "[]": "{}";
            }
            else
            {
                serialized = JsonConvert.SerializeObject(value, serializeSettings);
            }
                
            return JsonDocument.Parse(serialized);
        }
    }
}