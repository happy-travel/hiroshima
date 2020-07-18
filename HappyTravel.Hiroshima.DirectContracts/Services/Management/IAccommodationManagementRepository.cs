using System.Collections.Generic;
using System.Threading.Tasks;
using HappyTravel.Hiroshima.Data.Models.Accommodations;
using HappyTravel.Hiroshima.Data.Models.Rooms;

namespace HappyTravel.Hiroshima.DirectContracts.Services.Management
{
    public interface IAccommodationManagementRepository
    {
        Task<Accommodation> GetAccommodation(int contractManagerId, int accommodationId);
        Task<Accommodation> AddAccommodation(Accommodation accommodation);
        Task DeleteAccommodationAndRooms(int contractManagerId, int accommodationId);
        Task UpdateAccommodation(Accommodation accommodation);
        Task<List<Room>> GetRooms(int accommodationId);
        Task<List<Room>> GetRooms(int contractManagerId, int accommodationId);
        Task<List<Room>> AddRooms(List<Room> rooms);
        Task UpdateRooms(List<Room> rooms);
        Task DeleteRooms(List<int> roomIds);

    }
}