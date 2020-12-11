using System.Collections.Generic;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Common.Models.Availabilities;

namespace HappyTravel.Hiroshima.DirectContracts.Services.Availability
{
    public interface IAvailabilityHashGenerator
    {
        string Generate(List<RateDetails> rateDetails);
    }
}