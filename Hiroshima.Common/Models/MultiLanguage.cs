using Hiroshima.Common.Constants;

namespace Hiroshima.Common.Models
{
    public class MultiLanguage<T>
    {
        public T Ar { get; set; }
        public T En { get; set; }
        public T Ru { get; set; }


        public bool TryGetValue(string languageCode, out T value)
        {
            switch (languageCode.ToLowerInvariant())
            {
                case "ar":
                    value = Ar;
                    break;
                case "en":
                    value = En;
                    break;
                case "ru":
                    value = Ru;
                    break;
                default:
                    value = default;
                    return false;
            }

            return value != null;
        }


        public bool TryGetValueOrDefault(string languageCode, out T value)
        {
            if (TryGetValue(languageCode, out value))
                return true;

            var defaultLanguageCode = Languages.GetLanguageCode(Languages.DefaultLanguage);

            return TryGetValue(defaultLanguageCode, out value);
        }


        public bool TryAddValue(string languageCode, T value)
        {
            if (!Languages.TryGetLanguage(languageCode, out var language))
                return false;

            return TryAddValue(language, value);
        }


        public bool TryAddValue(DcLanguages etgLanguage, T value)
        {
            switch (etgLanguage)
            {
                case DcLanguages.Arabic:
                    En = value;
                    return true;
                case DcLanguages.English:
                    En = value;
                    return true;
                case DcLanguages.Russian:
                    Ru = value;
                    return true;
                default:
                    return false;
            }
        }
    }
}