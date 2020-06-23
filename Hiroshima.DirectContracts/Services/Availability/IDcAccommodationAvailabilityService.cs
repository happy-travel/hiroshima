using System.Collections.Generic;
using System.Threading.Tasks;
using HappyTravel.EdoContracts.Accommodations;
using Hiroshima.DbData.Models;

namespace Hiroshima.DirectContracts.Services.Availability
{
    public interface IDcAccommodationAvailabilityService
    {
        Task<List<AccommodationWithLocation>> GetAccommodations(AvailabilityRequest availabilityRequest, string  languageCode);
    }
}