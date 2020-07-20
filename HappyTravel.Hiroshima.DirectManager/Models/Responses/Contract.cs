using System;

namespace HappyTravel.Hiroshima.DirectManager.Models.Responses
{
    public struct Contract
    {
        public int Id { get; set; }
        public int AccommodationId { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}