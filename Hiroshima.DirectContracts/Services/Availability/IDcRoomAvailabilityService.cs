using System.Collections.Generic;
using System.Threading.Tasks;
using HappyTravel.EdoContracts.Accommodations;
using Hiroshima.DbData.Models;
using Hiroshima.DirectContracts.Models;

namespace Hiroshima.DirectContracts.Services.Availability
{
    public interface IDcRoomAvailabilityService
    {
        Task<List<RoomsGroupedByOccupation>> GetGroupedRooms(
            List<AccommodationWithLocation> accommodations, AvailabilityRequest availabilityRequest, string languageCode);
    }
}