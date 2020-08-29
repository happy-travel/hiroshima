using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public interface IAllocationRequirementManagementService
    {
        Task<Result<List<Models.Responses.AllocationRequirement>>> Add(int contactId, List<Models.Requests.AllocationRequirement> allocationRequirements);
        Task<Result<List<Models.Responses.AllocationRequirement>>> Get(int contractId, int skip, int top, List<int> roomIds, List<int> seasonIds, List<int> seasonRangeIds);
        Task<Result> Remove(int contractId, List<int> allocationRequirementIds);
    }
}