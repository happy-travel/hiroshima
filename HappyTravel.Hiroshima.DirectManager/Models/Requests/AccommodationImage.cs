using Microsoft.AspNetCore.Http;

namespace HappyTravel.Hiroshima.DirectManager.Models.Requests
{
    public class AccommodationImage
    {
        public int AccommodationId { get; set; }
        public FormFile UploadedFile { get; set; }
    }
}
