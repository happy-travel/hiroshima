using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json.Serialization;
using HappyTravel.Hiroshima.Common.Constants;
using Newtonsoft.Json;

namespace HappyTravel.Hiroshima.Common.Models
{
    public class MultiLanguage<T>
    {
        public bool TryGetValue(string languageCode, out T value)
        {
            value = default;

            return Languages.TryGetLanguage(languageCode, out var dcLanguage) && 
                   _languageStore.TryGetValue(dcLanguage, out value);
        }


        public bool TryGetValueOrDefault(string languageCode, out T value)
        { 
            if (TryGetValue(languageCode, out value))
                return true;

            var defaultLanguageCode = Languages.GetLanguageCode(Languages.DefaultLanguage);

            return TryGetValue(defaultLanguageCode, out value);
        }


        public IEnumerable<T> GetValues() => _languageStore.Values.Where(i => i != null); 


        public bool TrySetValue(string languageCode, T value)
        {
            if (!Languages.TryGetLanguage(languageCode, out var language))
                return false;
            
            SetValue(language, value);
            
            return true;
        }


        public void SetValue(DcLanguages dcLanguage, T value)
        {
            if (!_languageStore.TryAdd(dcLanguage, value))
                _languageStore[dcLanguage] = value;
        }


        public List<(string languageCode, T value)> GetAll()
            => _languageStore.Select(kv => (Languages.GetLanguageCode(kv.Key), kv.Value)).ToList();


        private T GetValue(DcLanguages language)
        {
            _languageStore.TryGetValue(language, out var value);
            return value;
        }
        
        [JsonPropertyName("ar")]
        public T Ar
        {
            get => GetValue(DcLanguages.Arabic);
            set => SetValue(DcLanguages.Arabic, value);
        }
        
        [JsonPropertyName("en")]
        public T En
        {
            get => GetValue(DcLanguages.English);
            set => SetValue(DcLanguages.English, value);
        }
        
        [JsonPropertyName("ru")]
        public T Ru
        {
            get => GetValue(DcLanguages.Russian);
            set => SetValue(DcLanguages.Russian, value);
        }

        
        public override bool Equals(object? obj) => obj is MultiLanguage<T> other && Equals(other);


        public bool Equals(MultiLanguage<T> other) 
            => JsonConvert.SerializeObject(this, _serializeSettings).Equals(JsonConvert.SerializeObject(other,_serializeSettings));


        public override int GetHashCode() => HashCode.Combine(JsonConvert.SerializeObject(this, _serializeSettings));
            
       
        private readonly Dictionary<DcLanguages, T> _languageStore = new Dictionary<DcLanguages, T>();
        
        
        private readonly JsonSerializerSettings _serializeSettings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
            MaxDepth = 5,
            Culture = CultureInfo.InvariantCulture
        };
    }
}