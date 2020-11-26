using System;
using System.Text.Json;
using HappyTravel.Hiroshima.Common.Models.Enums;

namespace HappyTravel.Hiroshima.Common.Models.Images
{
    public class Image
    {
        public Guid Id { get; set; }
        public OriginalImageDetails OriginalImageDetails { get; set; } = new OriginalImageDetails();
        public ImageKeys Keys { get; set; } = new ImageKeys();
        public DateTime Created { get; set; }
        public int ContractManagerId { get; set; }
        public int ReferenceId { get; set; }
        public ImageTypes ImageType { get; set; }
        public int Position { get; set; }
        public JsonDocument Description { get; set; }

        public ContractManager ContractManager { get; set;}
    }
}
