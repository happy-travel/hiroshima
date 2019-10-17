using Hiroshima.Common.Models;
using Hiroshima.DbData.Models.Enums;

namespace Hiroshima.DbData.Models.Common
{
    public class TextualDescription
    {
        public TextualDescriptionTypes Type { get; set; }
        public MultiLanguage<string> Description { get; set; }
    }
}
