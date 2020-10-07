using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace HappyTravel.Hiroshima.DirectManager.Models.Requests
{
    public class Document
    {
        //public string Name { get; set; }
        //public string MimeType { get; set; }
        public int ContractId { get; set; }
        //public byte[] FileContent { get; set; }
        public FormFile UploadedFile { get; set; }
    }
}
