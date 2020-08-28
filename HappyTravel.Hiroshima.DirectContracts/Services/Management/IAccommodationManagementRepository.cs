using System.Collections.Generic;
using System.Threading.Tasks;
using HappyTravel.Hiroshima.Data.Models.Accommodations;
using HappyTravel.Hiroshima.Data.Models.Rooms;

namespace HappyTravel.Hiroshima.DirectContracts.Services.Management
{
    public interface IAccommodationManagementRepository
    {
        Task<Accommodation> GetAccommodation(int contractManagerId, int accommodationId);
        Task<List<Room>> GetRooms(int accommodationId);

    }
}