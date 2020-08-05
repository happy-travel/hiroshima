using System;

namespace HappyTravel.Hiroshima.DirectContracts.Models
{
    public readonly struct CancellationPolicyDetails
    {
        public CancellationPolicyDetails(DateTime startDate, DateTime endDate, decimal price, string details = null)
        {
            StartDate = startDate;
            EndDate = endDate;
            Price = price;
            Details = details ?? string.Empty;
        }


        public DateTime StartDate { get; }
        public DateTime EndDate { get; }
        public decimal Price { get; }
        public string Details { get; }
    }
}