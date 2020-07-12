using System.Collections.Generic;
using System.Linq;
using HappyTravel.Hiroshima.Common.Constants;

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
        

        private T GetValue(DcLanguages language)
        {
            _languageStore.TryGetValue(language, out var value);
            return value;
        }
        
        
        public T Ar
        {
            get => GetValue(DcLanguages.Arabic);
            set => SetValue(DcLanguages.Arabic, value);
        }
        
        public T En
        {
            get => GetValue(DcLanguages.English);
            set => SetValue(DcLanguages.English, value);
        }
        
        public T Ru
        {
            get => GetValue(DcLanguages.Russian);
            set => SetValue(DcLanguages.Russian, value);
        }

        
        private readonly Dictionary<DcLanguages, T> _languageStore = new Dictionary<DcLanguages, T>();
    }
}