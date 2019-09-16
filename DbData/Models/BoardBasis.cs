using System;
using System.Collections.Generic;
using System.Text;

namespace Hiroshima.DbData.Models
{
    public class BoardBasis
    {
        public string Code { get; set; }
        public MultiLanguage<string> Name { get; set; }
        public IEnumerable<Rate> Rates { get; set; }
    }
}
