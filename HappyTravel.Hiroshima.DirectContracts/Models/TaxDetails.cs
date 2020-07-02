using HappyTravel.Money.Enums;

namespace HappyTravel.Hiroshima.DirectContracts.Models
{
    public struct TaxDetails
    {
        public string Name { get; set; }
        public Currencies Currency { get; set; }
        public decimal Price { get; set; }
    }
}