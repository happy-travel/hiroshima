using System;

namespace HappyTravel.DirectManager.Models.Requests
{
    public class Contract
    {
        public string AccommodationId { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}