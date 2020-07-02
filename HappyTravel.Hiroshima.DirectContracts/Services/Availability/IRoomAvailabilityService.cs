using System.Collections.Generic;
using System.Threading.Tasks;
using HappyTravel.EdoContracts.Accommodations;
using HappyTravel.Hiroshima.DirectContracts.Models;
using AccommodationDetails = HappyTravel.Hiroshima.DbData.Models.AccommodationDetails;

namespace HappyTravel.Hiroshima.DirectContracts.Services.Availability
{
    public interface IRoomAvailabilityService
    {
        Task<List<RoomsGroupedByOccupation>> GetGroupedRooms(
            List<AccommodationDetails> accommodations, AvailabilityRequest availabilityRequest, string languageCode);
    }
}