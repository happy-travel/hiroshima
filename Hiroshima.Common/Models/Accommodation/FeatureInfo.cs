using HappyTravel.EdoContracts.General.Enums;

namespace Hiroshima.Common.Models.Accommodation
{
    public class FeatureInfo
    {
        public bool IsCommentRequired { get; set; }
        public string Name { get; set; }
        public FieldTypes Type { get; set; }
    }
}