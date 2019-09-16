using Hiroshima.DbData.Models;
using Hiroshima.DirectContracts.Models.Common;

namespace Hiroshima.DirectContracts.Models.Responses
{
    public class DcAgreement
    {
        public MultiLanguage<string> RoomName { get; set; }
        public DcPrice DcPrice { get; set; }
        public DcPermittedOccupancy DcPermittedOccupancy { get; set; }
    }
}
