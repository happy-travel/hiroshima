using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public interface IAccommodationManagementService
    {
        public Task<Result<Models.Responses.Accommodation>> Get(int accommodationId);
       
        public Task<Result<Models.Responses.Accommodation>> Add(Models.Requests.Accommodation accommodation);
        
        public Task<Result> Remove(int accommodationId);

        Task<Result<Models.Responses.Accommodation>> Update(int accommodationId, Models.Requests.Accommodation accommodation);
        
        Task<Result<List<Models.Responses.Room>>> GetRooms(int accommodationId);
        
        Task<Result> RemoveRooms(int accommodationId, List<int> roomIds);
        
        Task<Result<List<Models.Responses.Room>>> AddRooms(int accommodationId, List<Models.Requests.Room> rooms);
    }
}