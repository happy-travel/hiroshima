using System;
using System.Collections.Generic;
using System.Text;

namespace HappyTravel.Hiroshima.DirectManager.Models.Requests
{
    public class SlimImage
    {
        public Guid Id { get; }
        public string LargeImageURL { get; }
        public string SmallImageURL { get; }
    }
}
