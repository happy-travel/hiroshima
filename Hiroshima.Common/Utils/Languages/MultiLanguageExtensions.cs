using System;
using Hiroshima.Common.Models;

namespace Hiroshima.Common.Utils.Languages
{
    public static class MultiLanguageExtensions
    {
        public static T TryGetValue<T>(this MultiLanguage<T> obj, Models.Enums.Language language)=>
            obj == null ? default : obj.GetValue(language);
    }
}
