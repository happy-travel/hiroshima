using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public interface IAccommodationManagementService
    {
        public Task<Result<Models.Responses.Accommodation>> GetAccommodation(int accommodationId);
        public Task<Result<Models.Responses.Accommodation>> AddAccommodation(Models.Requests.Accommodation accommodation);
        public Task<Result> RemoveAccommodation(int accommodationId);
        public Task<Result> Update(int accommodationId, Models.Requests.Accommodation accommodation);
    }
}