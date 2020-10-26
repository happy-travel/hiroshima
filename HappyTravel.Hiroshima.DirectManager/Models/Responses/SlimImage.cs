using HappyTravel.Hiroshima.Common.Models;
using System;

namespace HappyTravel.Hiroshima.DirectManager.Models.Responses
{
    public readonly struct SlimImage
    {
        public SlimImage(Guid id, string largeImageURL, string smallImageURL, MultiLanguage<string> description)
        {
            Id = id;
            LargeImageURL = largeImageURL;
            SmallImageURL = smallImageURL;
            Description = description;
        }

        public Guid Id { get; }
        public string LargeImageURL { get; }
        public string SmallImageURL { get; }
        public MultiLanguage<string> Description { get; }
    }
}
