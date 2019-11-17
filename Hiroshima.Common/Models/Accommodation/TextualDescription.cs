using HappyTravel.EdoContracts.Accommodations.Enums;

namespace Hiroshima.Common.Models.Accommodation
{
    public class TextualDescription
    {
        public TextualDescriptionTypes Type { get; set; }
        public MultiLanguage<string> Description { get; set; }
    }
}