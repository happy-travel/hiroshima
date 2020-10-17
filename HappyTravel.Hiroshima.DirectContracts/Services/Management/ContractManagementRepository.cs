using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Common.Models.Accommodations;
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace HappyTravel.Hiroshima.DirectContracts.Services.Management
{
    public class ContractManagementRepository : IContractManagementRepository
    {
        public ContractManagementRepository(DirectContractsDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<Contract> GetContract(int contractId, int contractManagerId)
        {
            var contract = await _dbContext.Contracts.SingleOrDefaultAsync(c => c.ContractManagerId == contractManagerId && c.Id == contractId);

            contract.Documents = await _dbContext.Documents.Where(d => d.ContractManagerId == contractManagerId && d.ContractId == contractId).ToListAsync();
            
            return contract;
        }
        //=> await _dbContext.Contracts.SingleOrDefaultAsync(c => c.ContractManagerId == contractManagerId && c.Id == contractId);


        public async Task<List<Accommodation>> GetRelatedAccommodations(int contractId, int contractManagerId) =>
            (await JoinContractAccommodationRelationAndAccommodation()
                .Where(contractAccommodationRelationAndAccommodation =>
                    contractAccommodationRelationAndAccommodation.Accommodation!.ContractManagerId == contractManagerId &&
                    contractAccommodationRelationAndAccommodation.ContractAccommodationRelation!.ContractId ==
                    contractId)
                .Select(contractAccommodationRelationAndAccommodation => contractAccommodationRelationAndAccommodation
                    .Accommodation)
                .ToListAsync())!;

        
        public async Task<List<ContractAccommodationRelation>> GetContractRelations(int contractManagerId, List<int> contractIds)
            => (await JoinContractAccommodationRelationAndAccommodation()
                    .Where(contractAccommodationRelationAndAccommodation =>
                        contractAccommodationRelationAndAccommodation.Accommodation!.ContractManagerId == contractManagerId &&
                        contractIds.Contains(contractAccommodationRelationAndAccommodation.ContractAccommodationRelation!.ContractId))
                    .Select(contractAccommodationRelationAndAccommodation =>
                        contractAccommodationRelationAndAccommodation.ContractAccommodationRelation).ToListAsync())!;

        
        private IQueryable<ContractAccommodationRelationAndAccommodation> JoinContractAccommodationRelationAndAccommodation() 
            => _dbContext.ContractAccommodationRelations.Join(_dbContext.Accommodations,
                contractAccommodationRelation => contractAccommodationRelation.AccommodationId,
                accommodation => accommodation.Id,
                (contractAccommodationRelation, accommodation) => new ContractAccommodationRelationAndAccommodation
                {
                    ContractAccommodationRelation = contractAccommodationRelation, Accommodation = accommodation
                });


        private readonly DirectContractsDbContext _dbContext;

        
        private class ContractAccommodationRelationAndAccommodation
        {
            public ContractAccommodationRelation? ContractAccommodationRelation { get; set; }
            public Accommodation? Accommodation { get; set; }
        }
    }
}