using System;
using System.Collections.Generic;
using System.Text.Json;
using Hiroshima.Common.Constants;
using Hiroshima.Common.Models.Enums;
using Hiroshima.Common.Utils.Languages;

namespace Hiroshima.Common.Models
{
    public class MultiLanguage<T>
    {
        public T Ar { get; set; }
        public T Cn { get; set; }
        public T De { get; set; }
        public T En { get; set; }
        public T Es { get; set; }
        public T Fr { get; set; }
        public T Ru { get; set; }


        public List<T> GetValues()
        {
            var values = new List<T>();
            foreach (var languageCode in ConstantValues.AvailableLanguages)
            {
                var language = GetValue(languageCode.Value);
                if (language != null)
                    values.Add(language);
            }

            return values;
        }


        public T GetValue(Language language)
            => language switch
                {
                    Language.Ar => Ar,
                    Language.Cn => Cn,
                    Language.De => De,
                    Language.En => En,
                    Language.Es => Es,
                    Language.Fr => Fr,
                    Language.Ru => Ru,
                _ => throw new ArgumentException("invalid enum value", nameof(language)),
                };


        public bool TryGetLanguageValue(string languageCode, out T value)
        {
            var language = LanguageUtils.GetLanguage(languageCode);
            if (language == Language.Unknown)
            {
                value = default;
                return false;
            }

            value = GetValue(language);
            return !(value is null);
        }


        public string GetJson() => JsonSerializer.Serialize(this, JsonSerializerOptions);


        private static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
        {
            IgnoreNullValues = true
        };
    }
}