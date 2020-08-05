using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public interface ISeasonManagementService
    {
        Task<Result<List<Models.Responses.SeasonResponse>>> Get(int contractId);
        
        Task<Result<List<Models.Responses.SeasonResponse>>> Replace(int contractId, List<Models.Requests.SeasonRequest> seasons);

        Task<Result> Remove(int contractId, List<int> seasonIds);
    }
}