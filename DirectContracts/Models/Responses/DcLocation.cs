using Hiroshima.DbData.Models;
using NetTopologySuite.Geometries;

namespace Hiroshima.DirectContracts.Models.Responses
{
    public class DcLocation
    {
        public MultiLanguage<string> Address { get; set; }
        public MultiLanguage<string> Country { get; set; }
        public MultiLanguage<string> Locality { get; set; }
        public Point Coordinates { get; set; }
        public Picture Picture { get; set; }
    }
}
