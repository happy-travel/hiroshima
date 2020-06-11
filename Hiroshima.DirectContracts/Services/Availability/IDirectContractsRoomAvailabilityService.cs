using System.Collections.Generic;
using System.Threading.Tasks;
using HappyTravel.EdoContracts.Accommodations;
using Hiroshima.DbData.Models;
using Hiroshima.DirectContracts.Models;
using Hiroshima.DirectContracts.Models.Internal;

namespace Hiroshima.DirectContracts.Services.Availability
{
    public interface IDirectContractsRoomAvailabilityService
    {
        Task<List<RoomsGroupedByOccupationRequest>> GetGroupedRooms(
            List<AccommodationData> accommodations, AvailabilityRequest availabilityRequest);
    }
}