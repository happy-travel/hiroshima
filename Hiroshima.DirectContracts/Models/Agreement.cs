using Hiroshima.Common.Models;
using Hiroshima.DbData.Models;
using PermittedOccupancy = Hiroshima.Common.Models.PermittedOccupancy;

namespace Hiroshima.DirectContracts.Models
{
    public class Agreement
    {
        public MultiLanguage<string> RoomName { get; set; }
        public Price Price { get; set; }
        public PermittedOccupancy PermittedOccupancy { get; set; }
    }
}
