using System;
using System.Collections.Generic;
using System.Text;

namespace HappyTravel.Hiroshima.DirectManager.Models.Requests
{
    public class Image
    {
        public string Name { get; set; }
        public string MimeType { get; set; }
        public int AccommodationId { get; set; }
        public byte[] FileContent { get; set; }
    }
}
