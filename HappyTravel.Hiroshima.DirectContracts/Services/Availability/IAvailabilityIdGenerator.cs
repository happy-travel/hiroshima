using System.Collections.Generic;
using HappyTravel.Hiroshima.Common.Models;

namespace HappyTravel.Hiroshima.DirectContracts.Services.Availability
{
    public interface IAvailabilityIdGenerator
    {
        string Generate(List<RateDetails> rateDetails);
    }
}