using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public interface IRateManagementService
    {
        Task<Result<List<Models.Responses.Rate>>> Get(int contractId, List<int> roomIds = null, List<int> seasonIds = null);
        Task<Result<List<Models.Responses.Rate>>> Add(int contractId, List<Models.Requests.Rate> rates);
        Task<Result> Remove(int contractId, List<int> rateIds);
    }
}