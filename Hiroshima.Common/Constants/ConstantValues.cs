using System.Collections.Generic;
using Hiroshima.Common.Models.Enums;

namespace Hiroshima.Common.Constants
{
    public class ConstantValues
    {
        public static readonly Dictionary<string, Languages> AvailableLanguages = new Dictionary<string, Languages>
        {
            {"ar", Languages.Ar},
            {"cn", Languages.Cn},
            {"de", Languages.De},
            {"en", Languages.En},
            {"es", Languages.Es},
            {"fr", Languages.Fr},
            {"ru", Languages.Ru}
        };
    }
}
