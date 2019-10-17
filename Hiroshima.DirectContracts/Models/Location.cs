using Hiroshima.Common.Models;
using Hiroshima.DbData.Models.Common;
using NetTopologySuite.Geometries;

namespace Hiroshima.DirectContracts.Models
{
    public class Location
    {
        public MultiLanguage<string> Address { get; set; }
        public MultiLanguage<string> Country { get; set; }
        public MultiLanguage<string> Locality { get; set; }
        public Point Coordinates { get; set; }
        public Picture Picture { get; set; }
    }
}
