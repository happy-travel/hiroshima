using Hiroshima.DbData.Models;
using Hiroshima.DirectContracts.Models.Enums;
using NetTopologySuite.Geometries;

namespace Hiroshima.DirectContracts.Models.Responses
{
    public class DcSearchLocation
    {
        public MultiLanguage<string> AccommodationName { get; set; }
        public MultiLanguage<string> Address { get; set; }
        public MultiLanguage<string> Country { get; set; }
        public MultiLanguage<string> Locality { get; set; }
        public Point Coordinates { get; set; }
        public LocationTypes Type { get; set; }
    }
}
