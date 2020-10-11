using System;

namespace HappyTravel.Hiroshima.DirectManager.Models.Responses
{
    public readonly struct Document
    {
        public Document(Guid id, string name, string key, string mimeType, int contractId)
        {
            Id = id;
            Name = name;
            Key = key;
            MimeType = mimeType;
            ContractId = contractId;
        }

        public Guid Id { get; }
        public string Name { get; }
        public string Key { get; }
        public string MimeType { get; }
        public int ContractId { get; }
    }
}
