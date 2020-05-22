using HappyTravel.EdoContracts.Accommodations.Enums;
using Hiroshima.Common.Models;

namespace Hiroshima.DbData.Models.Accommodation
{
    public class TextualDescription
    {
        public TextualDescriptionTypes Type { get; set; }
        public string Description { get; set; }
    }
}
