using System;

namespace HappyTravel.Hiroshima.Common.Models
{
    public class Document
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ContentType { get; set; }
        public string Key { get; set; }
        public DateTime Created { get; set; }
        public int ContractManagerId { get; set; }
        public int ContractId { get; set; }

        public Contract Contract { get; set; }
    }
}
