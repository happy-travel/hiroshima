using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.DirectManager.Models.Responses;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public interface IContractManagementService
    {
        public Task<Result<Contract>> GetContract(int contractId);
        public Task<Result<List<Contract>>> GetContracts();
        public Task<Result<Contract>> AddContract(Models.Requests.Contract contract);
        public Task<Result> UpdateContract(int contractId, Models.Requests.Contract contract);
        public Task<Result> DeleteContract(int contractId);
    }
}