using Hiroshima.Common.Models;
using Hiroshima.Common.Models.Enums;

namespace Hiroshima.Common.Utils.Languages
{
    public static class LanguageUtils
    {
        public static Language GetLanguage(string languageCode) =>
            Constants.ConstantValues.AvailableLanguages.TryGetValue(languageCode, out var language)
                ? language
                : Language.Unknown;


        public static bool TryGetLanguageValue<T>(MultiLanguage<T> obj, Language language, out T result)
        {
            result = default;
            if (obj is null)
                return false;
            result = obj.GetValue(language);
            return result != null;
        }

    }
}
