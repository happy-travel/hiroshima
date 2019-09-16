using System;
using System.Collections.Generic;
using System.Text;

namespace Hiroshima.DbData.Models
{
    public class Region
    {
        public int Id { get; set; }
        public MultiLanguage<string> Name { get; set; }
        public IEnumerable<Country> Countries { get; set; }
    }
}