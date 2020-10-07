using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace HappyTravel.Hiroshima.DirectManager.Models.Requests
{
    public class Image 
    {
        public string Name { get; }
        public string MimeType { get; }
        public int AccommodationId { get; set; }
        public byte[] FileContent { get; set; }
        //public FormFile UploadedFile { get; set; }
    }
}
