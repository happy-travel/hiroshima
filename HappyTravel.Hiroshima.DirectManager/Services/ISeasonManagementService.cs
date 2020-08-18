using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public interface ISeasonManagementService
    {
        Task<Result<List<Models.Responses.Season>>> Add(int contractId, List<string> names);
        Task<Result<List<Models.Responses.Season>>> Get(int contractId);
        Task<Result> Remove(int contractId, int seasonId);
        Task<Result<List<Models.Responses.SeasonRange>>> SetSeasonRanges(int contractId, List<Models.Requests.SeasonRange> seasonRanges);
        Task<Result<List<Models.Responses.SeasonRange>>> GetSeasonRanges(int contractId);
    }
}