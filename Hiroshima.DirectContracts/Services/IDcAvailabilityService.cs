using System.Collections.Generic;
using System.Threading.Tasks;
using HappyTravel.EdoContracts.Accommodations;
using Hiroshima.DirectContracts.Models;

namespace Hiroshima.DirectContracts.Services
{
    public interface IDcAvailabilityService
    {
        Task<List<AvailableAccommodation>> GetAvailable(AvailabilityRequest availabilityRequest, string languageCode);
    }
}