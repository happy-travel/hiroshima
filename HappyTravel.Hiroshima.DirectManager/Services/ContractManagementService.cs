using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using FluentValidation;
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.Data.Extensions;
using HappyTravel.Hiroshima.Data.Models;
using HappyTravel.Hiroshima.DirectManager.Infrastructure;
using HappyTravel.Hiroshima.DirectManager.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public class ContractManagementService : IContractManagementService
    {
        public ContractManagementService(IContractManagerContextService contractManagerContextService,
            DirectContracts.Services.Management.IContractManagementRepository contractManagementRepository,
            DirectContractsDbContext dbContext)
        {
            _contractManagerContext = contractManagerContextService;
            _contractManagementRepository = contractManagementRepository;
            _dbContext = dbContext;
        }


        public Task<Result<Models.Responses.Contract>> Get(int contractId)
        {
            return _contractManagerContext.GetContractManager().Bind(contractManager => Get(contractManager.Id));


            async Task<Result<Models.Responses.Contract>> Get(int contractManagerId)
            {
                var contract = await _contractManagementRepository.GetContract(contractId, contractManagerId);

                if (contract is null)
                    return Result.Failure<Models.Responses.Contract>($"Failed to get the contract with {nameof(contractId)} '{contractId}'");

                var accommodationId = (await _contractManagementRepository.GetRelatedAccommodations(contractId, contractManagerId)).Single().Id;

                return Build(contract, accommodationId);
            }
        }


        public Task<Result<List<Models.Responses.Contract>>> GetContracts(int skip = 0, int top = 100)
        {
           return _contractManagerContext.GetContractManager()
                .Map(Get);


            async Task<List<Models.Responses.Contract>> Get(ContractManager contractManager)
            {
                var contracts = await _dbContext.Contracts
                        .Where(contract => contract.ContractManagerId == contractManager.Id).OrderBy(contract => contract.Id)
                        .Skip(skip)
                        .Take(top)
                        .ToListAsync();

                    if (!contracts.Any())
                        return new List<Models.Responses.Contract>();

                    var contractIds = contracts.Select(contract => contract.Id).ToList();
                    var contractsAccommodationRelations =
                        (await _contractManagementRepository.GetContractRelations(contractManager.Id, contractIds)).ToDictionary(relation => relation.ContractId);

                    return contracts.Select(contract =>
                    {
                        var accommodationId = -1;
                        if (contractsAccommodationRelations.TryGetValue(contract.Id, out var relation))
                            accommodationId = relation.AccommodationId;
                        
                        return Build(contract, accommodationId);
                    }).ToList();
            }
        }


        public Task<Result<Models.Responses.Contract>> Add(Models.Requests.Contract contract)
        {
            return _contractManagerContext.GetContractManager()
                .EnsureAccommodationBelongsToContractManager(_dbContext, contract.AccommodationId)
                .Bind(contractManager =>
                {
                    var (_, failure, error) = Validate(contract);
                    
                    return failure ? Result.Failure<ContractManager>(error) : Result.Success(contractManager);
                })
                .Map(contractManager => Create(contractManager.Id, contract))
                .Map(Add)
                .Map(dbContract => Build(dbContract, contract.AccommodationId));

            
           async Task<Contract> Add(Contract dbContract)
            {
                var entry = _dbContext.Contracts.Add(dbContract);
                await _dbContext.SaveChangesAsync();
                
                _dbContext.DetachEntry(entry.Entity);

                var contractAccommodationRelationEntry = _dbContext.ContractAccommodationRelations.Add(
                    new ContractAccommodationRelation
                    {
                        ContractId = entry.Entity.Id, AccommodationId = contract.AccommodationId
                    });
                await _dbContext.SaveChangesAsync();
                
                _dbContext.DetachEntry(contractAccommodationRelationEntry.Entity);

                return entry.Entity;
            }
        }


        public async Task<Result> Update(int contractId, Models.Requests.Contract contract)
        {
            return await _contractManagerContext.GetContractManager()
                .EnsureContractBelongsToContractManager(_dbContext, contractId)
                .EnsureAccommodationBelongsToContractManager(_dbContext, contract.AccommodationId)
                .Bind(contractManager =>
                {
                    var (_, failure, error) = Validate(contract);

                    return failure ? Result.Failure<ContractManager>(error) : Result.Success(contractManager);
                })
                .Map(contractManager => Create(contractManager.Id, contract))
                .Map(Update);
               

            async Task Update(Contract dbContract)
            {
                dbContract.Id = contractId;
                var entry = _dbContext.Contracts.Update(dbContract);
                await _dbContext.SaveChangesAsync();
                
                _dbContext.DetachEntry(entry.Entity);
            }
        }


        public async Task<Result> Remove(int contractId)
        {
            return await _contractManagerContext.GetContractManager()
                .Tap(async contractManager => await RemoveContract(contractManager.Id));
               
            
            async Task RemoveContract(int contractManagerId)
            {
                var contract = await _dbContext.Contracts.SingleOrDefaultAsync(c => c.Id == contractId && c.ContractManagerId == contractManagerId);
                if (contract is null)
                    return;

                _dbContext.Contracts.Remove(contract);

                DeleteContractAccommodationRelations();
                
                await _dbContext.SaveChangesAsync();
            }
            
            
            async void DeleteContractAccommodationRelations()
            {
                var relations = await _dbContext.ContractAccommodationRelations
                    .Where(relation => relation.ContractId == contractId)
                    .ToListAsync();
                
                if (relations.Any())
                    _dbContext.ContractAccommodationRelations.RemoveRange(relations);
            }
        }

        
        private Contract Create(int contractManagerId, Models.Requests.Contract contract)
            => new Contract
            {
                Name = contract.Name,
                Description = contract.Description,
                ValidFrom = contract.ValidFrom,
                ValidTo = contract.ValidTo,
                ContractManagerId = contractManagerId
            };
        
        
        private Result Validate(Models.Requests.Contract contract)
        {
            var result = GenericValidator<Models.Requests.Contract>.Validate(v =>
            {
                v.RuleFor(c => c.Name).NotEmpty();
                v.RuleFor(c => c.Description).NotEmpty();
                v.RuleFor(c => c.AccommodationId).NotEmpty();
                v.RuleFor(c => c.ValidFrom).LessThan(c => c.ValidTo);
            }, contract);
            return result;
        }

        
        private Models.Responses.Contract Build(Contract contract, int accommodationId)
            => new Models.Responses.Contract(contract.Id, accommodationId, contract.ValidFrom, contract.ValidTo, contract.Name, contract.Description);


        private readonly IContractManagerContextService _contractManagerContext;
        private readonly DirectContracts.Services.Management.IContractManagementRepository _contractManagementRepository;
        private readonly DirectContractsDbContext _dbContext;
    }
}