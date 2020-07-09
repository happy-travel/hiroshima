using System.Collections.Generic;
using System.Threading.Tasks;
using HappyTravel.Hiroshima.Data.Models;
using HappyTravel.Hiroshima.Data.Models.Accommodations;

namespace HappyTravel.Hiroshima.DirectContracts.Services.Management
{
    public interface IContractManagementService
    {
        Task<Contract> GetContract(int userId, int contractId);
        Task<List<Contract>> GetContracts(int userId);
        Task<Contract> AddContract(Contract contract, int accommodationId);
        Task UpdateContract(Contract contract);
        Task DeleteContract(int userId, int contractId);
        Task<List<Accommodation>> GetRelatedAccommodations(int userId, int contractId);
        Task<List<ContractAccommodationRelation>> GetContractRelations(int userId, List<int> contractIds);
    }
}