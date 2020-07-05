using System;

namespace HappyTravel.Hiroshima.DirectContracts.Models
{
    public struct CancellationPolicyDetails
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Price { get; set; }
        public string Details { get; set; }
    }
}