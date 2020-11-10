using System;
using System.Text.Json;
using HappyTravel.Hiroshima.Common.Models.Accommodations;

namespace HappyTravel.Hiroshima.Common.Models.Images
{
    public class Image
    {
        public Guid Id { get; set; }
        public OriginalImageDetails OriginalImageDetails { get; set; } = new OriginalImageDetails();
        public ImageDetails SmallImage { get; set; } = new ImageDetails();
        public ImageDetails MainImage { get; set; } = new ImageDetails();
        public DateTime Created { get; set; }
        public int ContractManagerId { get; set; }
        public int AccommodationId { get; set; }
        public int Position { get; set; }
        public JsonDocument Description { get; set; }

        public ContractManager ContractManager { get; set;}
        public Accommodation Accommodation { get; set; }
    }
}
