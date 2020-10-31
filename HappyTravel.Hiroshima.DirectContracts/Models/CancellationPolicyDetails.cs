using System;

namespace HappyTravel.Hiroshima.DirectContracts.Models
{
    public readonly struct CancellationPolicyDetails
    {
        public CancellationPolicyDetails(DateTime startDate, DateTime endDate, decimal pricePenalty, double percent, string details = null)
        {
            StartDate = startDate;
            EndDate = endDate;
            PricePenalty = pricePenalty;
            Description = details ?? string.Empty;
            Percent = percent;
        }

        public override bool Equals(object? obj) => obj is CancellationPolicyDetails other && Equals(in other);


        public bool Equals(in CancellationPolicyDetails other) => (StartDate, EndDate, Price: PricePenalty, Details: Description).Equals((other.StartDate, other.EndDate, other.PricePenalty, other.Description));


        public override int GetHashCode() => HashCode.Combine(StartDate, EndDate, PricePenalty, Description);


        public DateTime StartDate { get; }
        public DateTime EndDate { get; }
        public decimal PricePenalty { get; }
        public double Percent { get; }
        public string Description { get; }
    }
}