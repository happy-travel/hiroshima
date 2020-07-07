using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.DirectManager.Models.Responses;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public interface IContractManagementService
    {
        public Task<Result<HappyTravel.DirectManager.Models.Responses.Contract>> GetContract(int contractId);
        public Task<Result<List<HappyTravel.DirectManager.Models.Responses.Contract>>> GetContracts();
        public Task<Result<int>> AddContract(Contract contract);
        public Task<Result> UpdateContract(int contractId, Contract accommodation);
        public Task<Result> DeleteContract(int contractId);
    }
}