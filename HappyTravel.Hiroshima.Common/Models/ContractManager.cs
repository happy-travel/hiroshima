using System;
using System.Collections.Generic;
using HappyTravel.Hiroshima.Common.Models.Accommodations;

namespace HappyTravel.Hiroshima.Common.Models
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
        
        public List<Accommodation> Accommodations { get; set; }
        
        public List<Contract> Contracts { get; set; }
    }
}