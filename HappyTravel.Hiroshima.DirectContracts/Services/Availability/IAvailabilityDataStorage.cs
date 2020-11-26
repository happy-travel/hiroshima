using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HappyTravel.Hiroshima.DirectContracts.Services.Availability
{
    public interface IAvailabilityDataStorage
    {
        Task Add(Common.Models.Availabilities.Availability availability);

        Task<string> GetHash(string availabilityId, Guid availableRateId);
    }
}