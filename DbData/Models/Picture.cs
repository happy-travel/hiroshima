using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Hiroshima.DbData.Models
{
    public class Picture
    {
        public string Source { get; set; }
        public MultiLanguage<string> Caption { get; set; }
    }
}
