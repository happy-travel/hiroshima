using System;

namespace HappyTravel.Hiroshima.Data.Models
{
    public class Contract
    {
        public int Id { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ContractManagerId { get; set; }
    }
}