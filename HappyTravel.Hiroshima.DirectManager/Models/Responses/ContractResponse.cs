using System;

namespace HappyTravel.Hiroshima.DirectManager.Models.Responses
{
    public readonly struct ContractResponse
    {
        public ContractResponse(int id, int accommodationId, DateTime validFrom, DateTime validTo, string name, string description)
        {
            Id = id;
            AccommodationId = accommodationId;
            ValidFrom = validFrom;
            ValidTo = validTo;
            Name = name;
            Description = description;
        }


        public int Id { get; }
        public int AccommodationId { get; }
        public DateTime ValidFrom { get; }
        public DateTime ValidTo { get; }
        public string Name { get; }
        public string Description { get; }
    }
}