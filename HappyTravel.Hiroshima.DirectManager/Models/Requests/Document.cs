using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace HappyTravel.Hiroshima.DirectManager.Models.Requests
{
    public class Document
    {
        public int ContractId { get; set; }
        public FormFile UploadedFile { get; set; }
    }
}
