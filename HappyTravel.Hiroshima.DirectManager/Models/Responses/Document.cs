using System;

namespace HappyTravel.Hiroshima.DirectManager.Models.Responses
{
    public readonly struct Document
    {
        public Document(Guid id, string name, string contentType, string key, int contractId)
        {
            Id = id;
            Name = name;
            ContentType = contentType;
            Key = key;
            ContractId = contractId;
        }

        public Guid Id { get; }
        public string Name { get; }
        public string Key { get; }
        public string ContentType { get; }
        public int ContractId { get; }
    }
}
