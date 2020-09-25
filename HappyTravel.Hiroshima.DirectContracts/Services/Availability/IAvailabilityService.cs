using System.Collections.Generic;
using System.Threading.Tasks;
using HappyTravel.EdoContracts.Accommodations;
using HappyTravel.Hiroshima.DirectContracts.Models;

namespace HappyTravel.Hiroshima.DirectContracts.Services.Availability
{
    public interface IAvailabilityService
    {
        Task<List<AvailableRates>> Get(AvailabilityRequest availabilityRequest, string languageCode);
    }
}