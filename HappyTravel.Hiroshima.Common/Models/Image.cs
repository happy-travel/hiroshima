using System;
using System.Text.Json;
using HappyTravel.Hiroshima.Common.Models.Accommodations;

namespace HappyTravel.Hiroshima.Common.Models
{
    public class Image
    {
        public Guid Id { get; set; }
        public string OriginalName { get; set; }
        public string OriginalContentType { get; set; }
        public string LargeImageKey { get; set; }
        public string SmallImageKey { get; set; }
        public string LargeImageUri { get; set; }
        public string SmallImageUri { get; set; }
        public DateTime Created { get; set; }
        public int ContractManagerId { get; set; }
        public int AccommodationId { get; set; }
        public int Position { get; set; }
        public JsonDocument Description { get; set; }
        
        public Accommodation Accommodation { get; set; }
    }
}
