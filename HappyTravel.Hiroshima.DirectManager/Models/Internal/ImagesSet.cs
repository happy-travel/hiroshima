using System;
using System.Collections.Generic;
using System.Text;

namespace HappyTravel.Hiroshima.DirectManager.Models.Internal
{
    public class ImagesSet
    {
        public string Name { get; set; }
        public string MimeType { get; set; }
        public byte[] LargeImage { get; set; }
        public byte[] SmallImage { get; set; }
    }
}
