using System.Collections.Generic;
using System.Threading.Tasks;
using HappyTravel.Hiroshima.Data.Models;
using HappyTravel.Hiroshima.Data.Models.Accommodations;

namespace HappyTravel.Hiroshima.DirectContracts.Services.Management
{
    public interface IContractManagementRepository
    {
        Task<Contract> GetContract(int contractManagerId, int contractId);
        Task<List<Contract>> GetContracts(int contractManagerId);
        Task<Contract> AddContract(Contract contract, int accommodationId);
        Task UpdateContract(Contract contract);
        Task DeleteContract(int contractManagerId, int contractId);
        Task<List<Accommodation>> GetRelatedAccommodations(int contractManagerId, int contractId);
        Task<List<ContractAccommodationRelation>> GetContractRelations(int contractManagerId, List<int> contractIds);
    }
}