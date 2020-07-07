using System.Collections.Generic;
using System.Threading.Tasks;
using HappyTravel.Hiroshima.Data.Models;
using HappyTravel.Hiroshima.Data.Models.Accommodations;
using HappyTravel.Hiroshima.Data.Models.Rooms;

namespace HappyTravel.Hiroshima.DirectContracts.Services.Management
{
    public interface IContractManagementService
    {
        Task<int> AddContract(Contract contract);
        Task<bool> UpdateContract(Contract contract);
        Task<Contract> GetContract(int contractId);
        Task<bool> DeleteContract(int contractId);
    }
}