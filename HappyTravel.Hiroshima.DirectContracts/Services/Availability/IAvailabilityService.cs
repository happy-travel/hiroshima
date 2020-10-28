using System.Collections.Generic;
using System.Threading.Tasks;
using HappyTravel.Hiroshima.Common.Models.Accommodations;
using HappyTravel.Hiroshima.DirectContracts.Models;

namespace HappyTravel.Hiroshima.DirectContracts.Services.Availability
{
    public interface IAvailabilityService
    {
        Task<Dictionary<Accommodation, List<AvailableRates>>> Get(EdoContracts.Accommodations.AvailabilityRequest availabilityRequest, string languageCode);
    }
}