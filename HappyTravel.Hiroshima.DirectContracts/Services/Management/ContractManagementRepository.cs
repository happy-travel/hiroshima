using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.Data.Models;
using HappyTravel.Hiroshima.Data.Models.Accommodations;
using Microsoft.EntityFrameworkCore;

namespace HappyTravel.Hiroshima.DirectContracts.Services.Management
{
    public class ContractManagementRepository : IContractManagementRepository
    {
        public ContractManagementRepository(DirectContractsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        
        public async Task<Contract> GetContract(int contractId, int contractManagerId) =>
            await _dbContext.Contracts.SingleOrDefaultAsync(c => c.ContractManagerId == contractManagerId && c.Id == contractId);

        
        public async Task<List<Contract>> GetContracts(int contractManagerId) =>
            await _dbContext.Contracts.Where(c => c.ContractManagerId == contractManagerId).ToListAsync();

        
        public async Task<Contract> AddContract(Contract contract, int accommodationId)
        {
            var contractEntry = _dbContext.Contracts.Add(contract);
            await _dbContext.SaveChangesAsync();
            contractEntry.State = EntityState.Detached;

            var contractAccommodationRelationEntry = _dbContext.ContractAccommodationRelations.Add(
                new ContractAccommodationRelation
                {
                    ContractId = contractEntry.Entity.Id, AccommodationId = accommodationId
                });
            await _dbContext.SaveChangesAsync();
            contractAccommodationRelationEntry.State = EntityState.Detached;

            return contractEntry.Entity;
        }

        
        public async Task UpdateContract(Contract contract)
        {
            var contractEntry = _dbContext.Contracts.Update(contract);
            await _dbContext.SaveChangesAsync();
            contractEntry.State = EntityState.Detached;
        }

        
        public async Task DeleteContract(int contractId, int contractManagerId)
        {
            await DeleteContractAccommodationRelations(contractId);

            var contract = await _dbContext.Contracts.SingleOrDefaultAsync(c => c.Id == contractId && c.ContractManagerId == contractManagerId);
            if (contract is null)
                return;
            
            var contractEntry = _dbContext.Contracts.Remove(contract);
            await _dbContext.SaveChangesAsync();
            contractEntry.State = EntityState.Detached;
        }

        
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

        
        private async Task DeleteContractAccommodationRelations(int contractId)
        {
            var contractAccommodationRelations = await _dbContext.ContractAccommodationRelations
                .Where(car => car.ContractId == contractId)
                .ToListAsync();
            if (contractAccommodationRelations.Any())
            {
                _dbContext.ContractAccommodationRelations.RemoveRange(contractAccommodationRelations);
                await _dbContext.SaveChangesAsync();
            }
        }

        
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