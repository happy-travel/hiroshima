using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.Data.Models;
using HappyTravel.Hiroshima.Data.Models.Accommodations;
using Microsoft.EntityFrameworkCore;

namespace HappyTravel.Hiroshima.DirectContracts.Services.Management
{
    public class ContractManagementService : IContractManagementService
    {
        public ContractManagementService(DirectContractsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Contract> GetContract(int userId, int contractId) =>
            await _dbContext.Contracts.SingleOrDefaultAsync(c => c.UserId == userId && c.Id == contractId);

        
        public async Task<List<Contract>> GetContracts(int userId) =>
            await _dbContext.Contracts.Where(c => c.UserId == userId).ToListAsync();

        
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

        
        public async Task DeleteContract(int userId, int contractId)
        {
            await DeleteContractAccommodationRelations(contractId);

            var contract = _dbContext.Contracts.SingleOrDefault(c => c.Id == contractId && c.UserId == userId);
            if (!(contract is null))
            {
                var contractEntry = _dbContext.Contracts.Remove(contract);
                await _dbContext.SaveChangesAsync();
                contractEntry.State = EntityState.Detached;
            }
        }

        
        public async Task<List<Accommodation>> GetRelatedAccommodations(int userId, int contractId) =>
            (await JoinContractAccommodationRelationAndAccommodation()
                .Where(contractAccommodationRelationAndAccommodation =>
                    contractAccommodationRelationAndAccommodation.Accommodation!.UserId == userId &&
                    contractAccommodationRelationAndAccommodation.ContractAccommodationRelation!.ContractId ==
                    contractId)
                .Select(contractAccommodationRelationAndAccommodation => contractAccommodationRelationAndAccommodation
                    .Accommodation)
                .ToListAsync())!;

        
        public async Task<List<ContractAccommodationRelation>> GetContractRelations(int userId, List<int> contractIds)
            => (await JoinContractAccommodationRelationAndAccommodation()
                    .Where(contractAccommodationRelationAndAccommodation =>
                        contractAccommodationRelationAndAccommodation.Accommodation!.UserId == userId &&
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

        
        class ContractAccommodationRelationAndAccommodation
        {
            public ContractAccommodationRelation? ContractAccommodationRelation { get; set; }
            public Accommodation? Accommodation { get; set; }
        }
    }
}