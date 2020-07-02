using System.Collections.Generic;
using System.Threading.Tasks;
using HappyTravel.EdoContracts.Accommodations;
using Hiroshima.DbData.Models;
using Hiroshima.DirectContracts.Models;
using AccommodationDetails = Hiroshima.DbData.Models.AccommodationDetails;

namespace Hiroshima.DirectContracts.Services.Availability
{
    public interface IRoomAvailabilityService
    {
        Task<List<RoomsGroupedByOccupation>> GetGroupedRooms(
            List<AccommodationDetails> accommodations, AvailabilityRequest availabilityRequest, string languageCode);
    }
}