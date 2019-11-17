using System.Collections.ObjectModel;
using Hiroshima.Common.Models;

namespace Hiroshima.DbData.Models.Location
{
    public class Region
    {
        public int Id { get; set; }
        public MultiLanguage<string> Name { get; set; }
        public Collection<Country> Countries { get; set; }
    }
}