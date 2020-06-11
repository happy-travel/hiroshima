using System;

namespace Hiroshima.DirectContracts.Models.Internal.Response
{
    public struct CancellationPolicyDetails
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Price { get; set; }
        public string Remark { get; set; }
    }
}