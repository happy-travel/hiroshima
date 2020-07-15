using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public interface IAccommodationManagementService
    {
        public Task<Result<Models.Responses.Accommodation>> GetAccommodation(int accommodationId);
        public Task<Result<Models.Responses.Accommodation>> AddAccommodation(Models.Requests.Accommodation accommodation);
        public Task<Result> RemoveAccommodation(int accommodationId);
        public Task<Result> UpdateAccommodation(int accommodationId, Models.Requests.Accommodation accommodation);
        Task<Result> RemoveRooms(int accommodationId, List<int> roomIds);
        Task<Result<List<Models.Responses.Room>>> AddRooms(int accommodationId, List<Models.Requests.Room> rooms);
    }
}