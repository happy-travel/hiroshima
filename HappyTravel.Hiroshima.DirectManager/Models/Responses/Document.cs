using System;

namespace HappyTravel.Hiroshima.DirectManager.Models.Responses
{
    public readonly struct Document
    {
        public Document(Guid uniqueId, string name, string key, string mimeType, int contractId)
        {
            UniqueId = uniqueId;
            Name = name;
            Key = key;
            MimeType = mimeType;
            ContractId = contractId;
        }

        public Guid UniqueId { get; }
        public string Name { get; }
        public string Key { get; }
        public string MimeType { get; }
        public int ContractId { get; }
    }
}
