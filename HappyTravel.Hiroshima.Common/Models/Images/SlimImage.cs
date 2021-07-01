using System;

namespace HappyTravel.Hiroshima.Common.Models.Images
{
    public class SlimImage
    {
        public Guid Id { get; set; }
        public string LargeImageURL { get; set; } = string.Empty;
        public string SmallImageURL { get; set; } = string.Empty;
        public MultiLanguage<string> Description { get; set; } = new MultiLanguage<string> { Ar = string.Empty, En = string.Empty, Ru = string.Empty };
    }
}
