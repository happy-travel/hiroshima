using System;
using System.Collections.Generic;

namespace HappyTravel.Hiroshima.DirectManager.Models.Responses
{
    public readonly struct Contract
    {
        public Contract(int id, int accommodationId, DateTime validFrom, DateTime validTo, string name, string description, List<Document> documents)
        {
            Id = id;
            AccommodationId = accommodationId;
            ValidFrom = validFrom;
            ValidTo = validTo;
            Name = name;
            Description = description;
            Documents = documents ?? new List<Document>();
        }


        public int Id { get; }
        public int AccommodationId { get; }
        public DateTime ValidFrom { get; }
        public DateTime ValidTo { get; }
        public string Name { get; }
        public string Description { get; }
        public List<Document> Documents { get; }
    }
}