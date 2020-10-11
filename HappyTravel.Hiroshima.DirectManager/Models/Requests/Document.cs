using Microsoft.AspNetCore.Http;

namespace HappyTravel.Hiroshima.DirectManager.Models.Requests
{
    public class Document
    {
        public int ContractId { get; set; }
        public FormFile UploadedFile { get; set; }
    }
}
