using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HappyTravel.Hiroshima.DirectContracts.Models;

namespace HappyTravel.Hiroshima.DirectContracts.Services.Availability
{
    public interface IAvailableRatesStorage
    {
        Task Add(List<AvailableRates> rates);

        ValueTask<AvailableRates> Get(Guid id);
    }
}