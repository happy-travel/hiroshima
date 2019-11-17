using System.Collections.Generic;
using Hiroshima.Common.Models;

namespace Hiroshima.DbData.Models.Location
{
    public class Locality
    {
        public int Id { get; set; }
        public MultiLanguage<string> Name { get; set; }
        public string CountryCode { get; set; }
        public Country Country { get; set; }
        public ICollection<Location> Locations { get; set; }
    }
}