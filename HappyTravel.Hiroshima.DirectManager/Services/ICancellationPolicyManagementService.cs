using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public interface ICancellationPolicyManagementService
    {
        Task<Result<List<Models.Responses.CancellationPolicy>>> Get(int contractId, int skip, int top, List<int> roomIds = null, List<int> seasonIds = null);
        Task<Result<List<Models.Responses.CancellationPolicy>>> Add(int contractId, List<Models.Requests.CancellationPolicy> cancellationPolicies);
        Task<Result> Remove(int contractId, List<int> cancellationPolicyIds);
    }
}