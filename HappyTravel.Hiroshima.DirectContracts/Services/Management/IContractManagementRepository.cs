using System.Collections.Generic;
using System.Threading.Tasks;
using HappyTravel.Hiroshima.Data.Models;
using HappyTravel.Hiroshima.Data.Models.Accommodations;

namespace HappyTravel.Hiroshima.DirectContracts.Services.Management
{
    public interface IContractManagementRepository
    {
        Task<Contract> GetContract(int contractId, int contractManagerId);
        Task<List<Accommodation>> GetRelatedAccommodations(int contractId, int contractManagerId);
        Task<List<ContractAccommodationRelation>> GetContractRelations(int contractManagerId, List<int> contractIds);
    }
}