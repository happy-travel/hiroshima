using HappyTravel.Money.Enums;

namespace HappyTravel.Hiroshima.DirectContracts.Models
{
    public readonly struct TaxDetails
    {
        public TaxDetails(string name, Currencies currency, decimal price)
        {
            Name = name;
            Currency = currency;
            Price = price;
        }


        public string Name { get; }
        public Currencies Currency { get; }
        public decimal Price { get; }
    }
}