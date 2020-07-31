using System;

namespace HappyTravel.Hiroshima.Data.Models
{
    public class ContractManager
    {
        public int Id { get; set; }
        public string IdentityHash { get; set; }
        public string Email { get; set; }
        public string? Title { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime Created { get; set; }
    }
}