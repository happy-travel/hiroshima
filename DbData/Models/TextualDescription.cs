using System;
using System.Collections.Generic;
using System.Text;
using Hiroshima.DbData.Models.Enums;
using Newtonsoft.Json;

namespace Hiroshima.DbData.Models
{
    public class TextualDescription
    {
        public TextualDescriptionTypes Type { get; set; }
        public MultiLanguage<string> Description { get; set; }
    }
}
