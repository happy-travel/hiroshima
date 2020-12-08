using HappyTravel.Hiroshima.Common.Models.Enums;

namespace HappyTravel.Hiroshima.Common.Models
{
    public class Picture
    {
        public string Source { get; set; }
        
        public PictureTypes Type { get; set; }
        
        public string Caption { get; set; }
    }
}