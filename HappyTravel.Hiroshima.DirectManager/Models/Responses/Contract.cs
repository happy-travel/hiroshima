using System;
using System.Collections.Generic;

namespace HappyTravel.DirectManager.Models.Responses
{
    public class Contract
    {
        public string Id { get; set; }
        public string AccommodationId { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}