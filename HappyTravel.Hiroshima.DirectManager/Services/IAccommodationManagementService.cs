using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public interface IAccommodationManagementService
    {
        public Task<Result<HappyTravel.DirectManager.Models.Responses.Accommodation>> GetAccommodation(int accommodationId);
        public Task<Result<int>> AddAccommodation(HappyTravel.DirectManager.Models.Requests.Accommodation accommodation);
        public Task<Result> DeleteAccommodation(string accommodationId);
        public Task<Result<HappyTravel.DirectManager.Models.Responses.Accommodation>> UpdateAccommodation(string accommodationId, HappyTravel.DirectManager.Models.Requests.Accommodation accommodation);
    }
}