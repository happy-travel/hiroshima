using System;
using HappyTravel.Money.Enums;
using HappyTravel.Money.Models;

namespace HappyTravel.Hiroshima.Common.Models.Availabilities
{
    public readonly struct TaxDetails
    {
        public TaxDetails(string name, Currencies currency, MoneyAmount amount)
        {
            Name = name;
            Currency = currency;
            Amount = amount;
        }


        public override bool Equals(object? obj) => obj is TaxDetails other && Equals(other);


        public bool Equals(in TaxDetails other) => (Name, Currency, Amount).Equals((other.Name, other.Currency, other.Amount));

        
        public override int GetHashCode() => HashCode.Combine(Name, Currency, Amount);
        
        
        public string Name { get; }
        public Currencies Currency { get; }
        public MoneyAmount Amount { get; }
    }
}