using System.Threading.Tasks;
using HappyTravel.Hiroshima.Data.Models;

namespace HappyTravel.Hiroshima.DirectContracts.Services.Management
{
    public class ContractManagementService: IContractManagementService
    {
        public Task<int> AddContract(Contract contract)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> UpdateContract(Contract contract)
        {
            throw new System.NotImplementedException();
        }

        public Task<Contract> GetContract(int contractId)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> DeleteContract(int contractId)
        {
            throw new System.NotImplementedException();
        }
    }
}