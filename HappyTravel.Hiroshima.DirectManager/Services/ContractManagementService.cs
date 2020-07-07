using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Contract = HappyTravel.DirectManager.Models.Responses.Contract;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public class ContractManagementService: IContractManagementService
    {
        public Task<Result<Contract>> GetContract(int contractId)
        {
            throw new System.NotImplementedException();
        }

        public Task<Result<List<Contract>>> GetContracts()
        {
            throw new System.NotImplementedException();
        }

        public Task<Result<int>> AddContract(Contract contract)
        {
            throw new System.NotImplementedException();
        }

        public Task<Result> UpdateContract(int contractId, Contract accommodation)
        {
            throw new System.NotImplementedException();
        }

        public Task<Result> DeleteContract(int contractId)
        {
            throw new System.NotImplementedException();
        }
    }
}