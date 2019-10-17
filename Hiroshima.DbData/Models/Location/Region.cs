using System.Collections.Generic;
using Hiroshima.Common.Models;

namespace Hiroshima.DbData.Models.Location
{
    public class Region
    {
        public int Id { get; set; }
        public MultiLanguage<string> Name { get; set; }
        public IEnumerable<Country> Countries { get; set; }
    }
}