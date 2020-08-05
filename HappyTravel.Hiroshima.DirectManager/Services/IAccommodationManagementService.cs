using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public interface IAccommodationManagementService
    {
        public Task<Result<Models.Responses.AccommodationResponse>> Get(int accommodationId);
       
        public Task<Result<Models.Responses.AccommodationResponse>> Add(Models.Requests.AccommodationRequest accommodationRequest);
        
        public Task<Result> Remove(int accommodationId);

        Task<Result<Models.Responses.AccommodationResponse>> Update(int accommodationId, Models.Requests.AccommodationRequest accommodationRequest);
        
        Task<Result<List<Models.Responses.RoomResponse>>> GetRooms(int accommodationId);
        
        Task<Result> RemoveRooms(int accommodationId, List<int> roomIds);
        
        Task<Result<List<Models.Responses.RoomResponse>>> AddRooms(int accommodationId, List<Models.Requests.RoomRequest> rooms);
    }
}