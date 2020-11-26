using Microsoft.AspNetCore.Http;

namespace HappyTravel.Hiroshima.DirectManager.Models.Requests
{
    public class RoomImage
    {
        public int AccommodationId { get; set; }
        public int RoomId { get; set; }
        public FormFile UploadedFile { get; set; }
    }
}
