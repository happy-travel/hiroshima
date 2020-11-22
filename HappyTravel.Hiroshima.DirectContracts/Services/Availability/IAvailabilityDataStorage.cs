using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HappyTravel.Hiroshima.DirectContracts.Models;

namespace HappyTravel.Hiroshima.DirectContracts.Services.Availability
{
    public interface IAvailabilityDataStorage
    {
        Task Add(Models.Availability availability);

        Task<string> GetHash(string availabilityId, Guid availableRateId);
    }
}