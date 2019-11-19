using System;
using System.Collections.Generic;
using Hiroshima.DbData.Models.Rates;
using Hiroshima.DirectContracts.Models.Internal;

namespace Hiroshima.DirectContracts.Services
{
    public interface IPricesService
    {
        List<SeasonPrice> GetSeasonPrices(ICollection<ContractedRate> contractRates, ICollection<DiscountRate> discountRates, DateTime checkInDate,
            DateTime checkOutDate);
    }
}