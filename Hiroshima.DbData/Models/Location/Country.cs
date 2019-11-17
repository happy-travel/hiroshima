using System.Collections.Generic;
using Hiroshima.Common.Models;

namespace Hiroshima.DbData.Models.Location
{
    public class Country
    {
        public string Code { get; set; }
        public MultiLanguage<string> Name { get; set; }
        public Region Region { get; set; }
        public int RegionId { get; set; }
        public ICollection<Locality> Localities { get; set; }
    }
}