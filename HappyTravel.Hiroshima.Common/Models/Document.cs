using System;

namespace HappyTravel.Hiroshima.Common.Models
{
    public class Document
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
        public DateTime Created { get; set; }
        public int ContractManagerId { get; set; }
        public int ContractId { get; set; }

        public Manager ContractManager { get; set; }
        public Contract Contract { get; set; }
    }
}
