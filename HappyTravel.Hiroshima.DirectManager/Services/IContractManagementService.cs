using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.DirectManager.Models.Responses;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public interface IContractManagementService
    {
        public Task<Result<ContractResponse>> Get(int contractId);
        public Task<Result<List<ContractResponse>>> Get();
        public Task<Result<ContractResponse>> Add(Models.Requests.ContractRequest contractRequest);
        public Task<Result> Update(int contractId, Models.Requests.ContractRequest contractRequest);
        public Task<Result> Remove(int contractId);
    }
}