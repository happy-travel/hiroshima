using System;
using System.Collections.Generic;
using System.Text;

namespace HappyTravel.Hiroshima.DirectManager.Models.Responses
{
    public readonly struct SlimImage
    {
        public SlimImage(Guid id, string largeImageURL, string smallImageURL)
        {
            Id = id;
            LargeImageURL = largeImageURL;
            SmallImageURL = smallImageURL;
        }

        public Guid Id { get; }
        public string LargeImageURL { get; }
        public string SmallImageURL { get; }
    }
}
