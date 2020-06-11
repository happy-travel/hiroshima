using System.Collections.Generic;
using HappyTravel.Money.Enums;
using Hiroshima.Common.Models;

namespace Hiroshima.DirectContracts.Models.Internal.Response
{
    public struct PaymentDetails
    {
        public decimal TotalPrice { get; set; }
        public List<decimal> DailyPrices { get; set; }
        public List<SeasonPrice> SeasonPrices { get; set; }
        public Currencies Currency { get; set; }
        public MultiLanguage<string> Details { get; set; }
    }
}