using Hiroshima.Common.Models;
using Hiroshima.Common.Models.Enums;

namespace Hiroshima.Common.Utils.Languages
{
    public static class LanguageUtils
    {
        public static Models.Enums.Languages GetLanguage(string languageCode) =>
            Constants.ConstantValues.AvailableLanguages.TryGetValue(languageCode, out var language)
                ? language
                : Models.Enums.Languages.Unknown;
    }
}
