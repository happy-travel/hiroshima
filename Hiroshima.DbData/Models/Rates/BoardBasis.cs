using System.Collections.Generic;
using Hiroshima.Common.Models;

namespace Hiroshima.DbData.Models.Rates
{
    public class BoardBasis
    {
        public string Code { get; set; }
        public MultiLanguage<string> Name { get; set; }
        public IEnumerable<Rate> Rates { get; set; }
    }
}
