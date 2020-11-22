using System;
using HappyTravel.Money.Models;

namespace HappyTravel.Hiroshima.Common.Models.Availabilities
{
    public readonly struct CancellationPolicyDetails
    {
        public CancellationPolicyDetails(DateTime startDate, DateTime endDate, MoneyAmount penaltyAmount, decimal percent, string description = null)
        {
            StartDate = startDate;
            EndDate = endDate;
            PenaltyAmount = penaltyAmount;
            Description = description ?? string.Empty;
            Percent = percent;
        }

        public override bool Equals(object? obj) => obj is CancellationPolicyDetails other && Equals(in other);


        public bool Equals(in CancellationPolicyDetails other) => (StartDate, EndDate, PenaltyAmount.Amount, PenaltyAmount.Currency, Description).Equals((other.StartDate, other.EndDate, other.PenaltyAmount.Amount, other.PenaltyAmount.Currency, other.Description));


        public override int GetHashCode() => HashCode.Combine(StartDate, EndDate, PenaltyAmount.Amount, PenaltyAmount.Currency, Description);


        public DateTime StartDate { get; }
        public DateTime EndDate { get; }
        public MoneyAmount PenaltyAmount { get; }
        public decimal Percent { get; }
        public string Description { get; }
    }
}