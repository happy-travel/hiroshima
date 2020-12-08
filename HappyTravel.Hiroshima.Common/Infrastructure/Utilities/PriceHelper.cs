using System.Collections.Generic;
using System.Linq;
using HappyTravel.EdoContracts.General;
using HappyTravel.Hiroshima.Common.Models.Availabilities;
using HappyTravel.Money.Helpers;
using HappyTravel.Money.Models;

namespace HappyTravel.Hiroshima.Common.Infrastructure.Utilities
{
    public static class PriceHelper
    {
        public static (MoneyAmount amount, Discount discount) GetPrice(List<RateDetails> rateDetails)
        {
            var firstRateDetails = rateDetails.First();
            var currency = firstRateDetails.PaymentDetails.TotalAmount.Currency;
            var totalPrice = rateDetails.Sum(rateDetailsItem => rateDetailsItem.PaymentDetails.TotalAmount.Amount); 
            var moneyAmount = new MoneyAmount(totalPrice, currency);

            var totalPriceWithDiscount = rateDetails.Sum(rateDetailsItem => rateDetailsItem.PaymentDetails.TotalAmount.Amount - rateDetailsItem.PaymentDetails.TotalAmount.Amount * rateDetailsItem.PaymentDetails.Discount.Percent / 100);
            var totalDiscount = new Discount(MoneyRounder.Truncate(100 - totalPriceWithDiscount * 100 / totalPrice, currency));

            return (moneyAmount, totalDiscount);
        }
    }
}