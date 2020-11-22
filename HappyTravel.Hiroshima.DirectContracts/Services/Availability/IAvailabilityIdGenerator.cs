using System.Collections.Generic;
using HappyTravel.Hiroshima.DirectContracts.Models;

namespace HappyTravel.Hiroshima.DirectContracts.Services.Availability
{
    public interface IAvailabilityIdGenerator
    {
        string Generate(List<RateDetails> rateDetails);
    }
}