using Hiroshima.Common.Models;
using Hiroshima.Common.Models.Enums;

namespace Hiroshima.Common.Utils.Languages
{
    public static class MultiLanguageExtensions
    {
        public static T TryGetValue<T>(this MultiLanguage<T> obj, Language language)
            => Equals(obj, null)
                ? default
                : obj.GetValue(language);
    }
}