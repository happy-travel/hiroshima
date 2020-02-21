using System.Collections.Generic;
using Hiroshima.Common.Models.Enums;

namespace Hiroshima.Common.Constants
{
    public class ConstantValues
    {
        public static readonly Dictionary<string, Language> AvailableLanguages = new Dictionary<string, Language>
        {
            {"ar", Language.Ar},
            {"cn", Language.Cn},
            {"de", Language.De},
            {"en", Language.En},
            {"es", Language.Es},
            {"fr", Language.Fr},
            {"ru", Language.Ru}
        };
    }
}