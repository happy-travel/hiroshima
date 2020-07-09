using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public interface IAccommodationManagementService
    {
        public Task<Result<Models.Responses.Accommodation>> Get(int accommodationId);
        public Task<Result<Models.Responses.Accommodation>> Add(Models.Requests.Accommodation accommodation);
        public Task<Result> Remove(string accommodationId);
        public Task<Result> Update(string accommodationId, Models.Requests.Accommodation accommodation);
    }
}