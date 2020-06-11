using System.Collections.Generic;
using System.Threading.Tasks;
using HappyTravel.EdoContracts.Accommodations;
using Hiroshima.DbData.Models;

namespace Hiroshima.DirectContracts.Services.Availability
{
    public interface IDirectContractsAccommodationAvailabilityService
    {
        Task<List<AccommodationData>> GetAccommodations(AvailabilityRequest availabilityRequest);
    }
}