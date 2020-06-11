using HappyTravel.Money.Enums;

namespace Hiroshima.DirectContracts.Models.Internal.Response
{
    public struct TaxDetails
    {
        public string Name { get; set; }
        public Currencies Currency { get; set; }
        public decimal Price { get; set; }
    }
}