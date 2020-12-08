using System;
using HappyTravel.Money.Models;

namespace HappyTravel.Hiroshima.Common.Models.Availabilities
{
    public readonly struct CancellationPolicyDetails
    {
        public CancellationPolicyDetails(DateTime fromDate, DateTime toDate, MoneyAmount penaltyAmount, decimal percent, string description = null)
        {
            FromDate = fromDate;
            ToDate = toDate;
            PenaltyAmount = penaltyAmount;
            Description = description ?? string.Empty;
            Percent = percent;
        }

        
        public override bool Equals(object? obj) => obj is CancellationPolicyDetails other && Equals(in other);


        public bool Equals(in CancellationPolicyDetails other) => (StartDate: FromDate, EndDate: ToDate, PenaltyAmount.Amount, PenaltyAmount.Currency, Description).Equals((other.FromDate, other.ToDate, other.PenaltyAmount.Amount, other.PenaltyAmount.Currency, other.Description));


        public override int GetHashCode() => HashCode.Combine(FromDate, ToDate, PenaltyAmount.Amount, PenaltyAmount.Currency, Description);


        public DateTime FromDate { get; }
        public DateTime ToDate { get; }
        public MoneyAmount PenaltyAmount { get; }
        public decimal Percent { get; }
        public string Description { get; }
    }
}