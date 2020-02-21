using Hiroshima.Common.Constants;
using Hiroshima.Common.Models.Enums;

namespace Hiroshima.Common.Utils.Languages
{
    public static class LanguageUtils
    {
        public static Language GetLanguage(string languageCode)
            => ConstantValues.AvailableLanguages.TryGetValue(languageCode, out var language)
                ? language
                : Language.Unknown;
    }
}