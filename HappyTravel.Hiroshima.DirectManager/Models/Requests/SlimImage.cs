using HappyTravel.Hiroshima.Common.Models;
using System;

namespace HappyTravel.Hiroshima.DirectManager.Models.Requests
{
    public class SlimImage
    {
        public Guid Id { get; set; }
        public string LargeImageURL { get; set; }
        public string SmallImageURL { get; set; }
        public MultiLanguage<string> Description { get; set; } = new MultiLanguage<string> { Ar = string.Empty, En = string.Empty, Ru = string.Empty };
    }
}
