using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.DirectManager.Models.Responses;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public interface IContractManagementService
    {
        public Task<Result<Contract>> Get(int contractId);
        public Task<Result<List<Contract>>> GetContracts(int skip = 0, int top = 100);
        public Task<Result<Contract>> Add(Models.Requests.Contract contract);
        public Task<Result> Update(int contractId, Models.Requests.Contract contract);
        public Task<Result> Remove(int contractId);
    }
}