using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public interface IAllocationRequirementManagementService
    {
        Task<Result<List<Models.Responses.AllocationRequirement>>> Add(int contactId, List<Models.Requests.AllocationRequirement> allocationRequirements);
    }
}