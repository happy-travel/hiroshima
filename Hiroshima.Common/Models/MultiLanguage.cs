using System;
using System.Collections.Generic;
using Hiroshima.Common.Models.Enums;
using Hiroshima.Common.Utils.Languages;

namespace Hiroshima.Common.Models
{
    public class MultiLanguage<T>
    {
        public List<T> GetValues()
        {
            var values = new List<T>();
            foreach (var languageCode in Constants.ConstantValues.AvailableLanguages)
            {
                var language = GetValue(languageCode.Value);
                if (language != null)
                    values.Add(language);
            }
            return values;
        }


        public T GetValue(Languages languages) =>
        languages switch
        {
            Languages.Ar => Ar,
            Languages.Cn => Cn,
            Languages.De => De,
            Languages.En => En,
            Languages.Es => Es,
            Languages.Fr => Fr,
            Languages.Ru => Ru,
            _ => throw new ArgumentException(message: "invalid enum value", paramName: nameof(languages)),
        };


        public bool TryGetValue(string languageCode, out T value)
        {
            var language = LanguageUtils.GetLanguage(languageCode);
            if (language == Languages.Unknown)
            {
                value = default;
                return false;
            }
            value = GetValue(language);
            return !(value is null);
        }

        
        public T Ar { get; set; }
        public T Cn { get; set; }
        public T De { get; set; }
        public T En { get; set; }
        public T Es { get; set; }
        public T Fr { get; set; }
        public T Ru { get; set; }

    }
}
