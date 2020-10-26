using Microsoft.AspNetCore.Http;

namespace HappyTravel.Hiroshima.DirectManager.Models.Requests
{
    public class Image 
    {
        public int AccommodationId { get; set; }
        public FormFile UploadedFile { get; set; }
    }
}
