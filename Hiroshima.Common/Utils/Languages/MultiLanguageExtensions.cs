using System;
using Hiroshima.Common.Models;

namespace Hiroshima.Common.Utils.Languages
{
    public static class MultiLanguageExtensions
    {
        public static T TryGetValue<T>(this MultiLanguage<T> obj, Models.Enums.Languages languages)=>
            obj == null ? default : obj.GetValue(languages);
    }
}
