using System;
using System.Collections.Generic;
using Hiroshima.Common.Models.Enums;

namespace Hiroshima.Common.Models
{
    public class MultiLanguage<T>
    {
        public T GetValue(string languageCode)
        {
            return GetValue(GetLanguage(languageCode));
        }


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


        public T GetValue(Language language)
        {
            switch (language)
            {
                case Language.Ar:
                    return Ar;
                case Language.Cn:
                    return Cn;
                case Language.De:
                    return De;
                case Language.En:
                    return En;
                case Language.Fr:
                    return Fr;
                case Language.Es:
                    return Es;
                case Language.Ru:
                    return Ru;
                default: throw new ArgumentOutOfRangeException();
            }
        }


        public static Language GetLanguage(string languageCode)
        {
            if (string.IsNullOrWhiteSpace(languageCode) ||
                !Constants.ConstantValues.AvailableLanguages.TryGetValue(languageCode, out var language))
                throw new ArgumentException($"Unknown {nameof(languageCode)}: {languageCode}");
            return language;
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
