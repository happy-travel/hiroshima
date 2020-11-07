using System.Collections.Generic;
using System.Threading.Tasks;
using HappyTravel.Hiroshima.DirectContracts.Models;

namespace HappyTravel.Hiroshima.DirectContracts.Services.Availability
{
    public interface IAvailabilityService
    {
        Task<Dictionary<Hiroshima.Common.Models.Accommodations.Accommodation, List<AvailableRates>>> Get(EdoContracts.Accommodations.AvailabilityRequest availabilityRequest, string languageCode);

        Task<Dictionary<Hiroshima.Common.Models.Accommodations.Accommodation, List<AvailableRates>>> Get(EdoContracts.Accommodations.AvailabilityRequest availabilityRequest, int accommodationId, string languageCode);
    }
}