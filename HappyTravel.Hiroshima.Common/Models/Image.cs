using System;

namespace HappyTravel.Hiroshima.Common.Models
{
    public class Image
    {
        public Guid Id { get; set; }
        public string OriginalName { get; set; }
        public string OriginalContentType { get; set; }
        public string LargeImageKey { get; set; }
        public string SmallImageKey { get; set; }
        public DateTime Created { get; set; }
        public int ContractManagerId { get; set; }
        public int AccommodationId { get; set; }
    }
}
