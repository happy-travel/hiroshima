﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Common.Models.Accommodations;
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.Data.Extensions;
using HappyTravel.Hiroshima.Data.Models;
using HappyTravel.Hiroshima.DirectManager.Infrastructure.Extensions;
using HappyTravel.Hiroshima.DirectManager.RequestValidators;
using Microsoft.EntityFrameworkCore;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public class ContractManagementService : IContractManagementService
    {
        public ContractManagementService(IContractManagerContextService contractManagerContextService, IDocumentManagementService documentManagementService,
            DirectContractsDbContext dbContext)
        {
            _contractManagerContext = contractManagerContextService;
            _documentManagementService = documentManagementService;
            _dbContext = dbContext;
        }


        public Task<Result<Models.Responses.Contract>> Get(int contractId)
        {
            return _contractManagerContext.GetContractManager()
                .Bind(contractManager => Get(contractManager.Id));


            async Task<Result<Models.Responses.Contract>> Get(int contractManagerId)
            {
                var contract = await GetContractWithDocuments(contractId, contractManagerId);

                if (contract is null)
                    return Result.Failure<Models.Responses.Contract>($"Failed to get the contract with {nameof(contractId)} '{contractId}'");

                var accommodationId = (await GetRelatedAccommodations(contractId, contractManagerId)).Single().Id;

                return Build(contract, accommodationId);
            }
        }


        public Task<Result<List<Models.Responses.Contract>>> GetContracts(int skip, int top)
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
                        (await GetContractRelations(contractManager.Id, contractIds)).ToDictionary(relation => relation.ContractId);

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
                    var validationResult = ValidationHelper.Validate(contract, new ContractValidator());
                    
                    return validationResult.IsFailure ? Result.Failure<ContractManager>(validationResult.Error) : Result.Success(contractManager);
                })
                .Map(contractManager => Create(contractManager.Id, contract))
                .Map(Add)
                .Map(dbContract => Build(dbContract, contract.AccommodationId));

            
           async Task<Contract> Add(Contract dbContract)
            {
                dbContract.Created = DateTime.UtcNow;
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
                    var (_, failure, error) = ValidationHelper.Validate(contract, new ContractValidator());

                    return failure ? Result.Failure<ContractManager>(error) : Result.Success(contractManager);
                })
                .Map(contractManager => Create(contractManager.Id, contract))
                .Tap(Update);
               

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
                .Tap(contractManager => RemoveContractDocuments(contractManager.Id))
                .Tap(async contractManager => await RemoveContract(contractManager.Id));

            async Task<Result> RemoveContractDocuments(int contractManagerId)
            {
                return await _documentManagementService.RemoveAll(contractManagerId, contractId);
            }

            async Task RemoveContract(int contractManagerId)
            {
                var contract = await _dbContext.Contracts.SingleOrDefaultAsync(c => c.Id == contractId && c.ContractManagerId == contractManagerId);
                if (contract is null)
                    return;

                DeleteContractAccommodationRelations();

                _dbContext.Contracts.Remove(contract);
                
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
                ValidFrom = contract.ValidFrom.Date,
                ValidTo = contract.ValidTo.Date,
                Modified = DateTime.UtcNow,
                ContractManagerId = contractManagerId
            };
        
        
        private Models.Responses.Contract Build(Contract contract, int accommodationId)
        {
            var contractDocuments = contract.Documents.Select(document => new Models.Responses.Document
                (
                    document.Id,
                    document.Name,
                    document.ContentType,
                    document.Key,
                    document.ContractId
                )).ToList();

            return new Models.Responses.Contract(
                contract.Id,
                accommodationId,
                contract.ValidFrom,
                contract.ValidTo,
                contract.Name,
                contract.Description,
                contractDocuments);
        }


        private async Task<Contract> GetContractWithDocuments(int contractId, int contractManagerId)
        {
            var contract = await _dbContext.Contracts.SingleOrDefaultAsync(c => c.ContractManagerId == contractManagerId && c.Id == contractId);
            if (contract == null)
                return contract;

            contract.Documents = await _dbContext.Documents.Where(d => d.ContractManagerId == contractManagerId && d.ContractId == contractId).ToListAsync();

            return contract;
        }


        private async Task<List<Accommodation>> GetRelatedAccommodations(int contractId, int contractManagerId) =>
            (await JoinContractAccommodationRelationAndAccommodation()
                .Where(contractAccommodationRelationAndAccommodation =>
                    contractAccommodationRelationAndAccommodation.Accommodation!.ContractManagerId == contractManagerId &&
                    contractAccommodationRelationAndAccommodation.ContractAccommodationRelation!.ContractId ==
                    contractId)
                .Select(contractAccommodationRelationAndAccommodation => contractAccommodationRelationAndAccommodation.Accommodation)
                .ToListAsync())!;


        private async Task<List<ContractAccommodationRelation>> GetContractRelations(int contractManagerId, List<int> contractIds)
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
                        ContractAccommodationRelation = contractAccommodationRelation,
                        Accommodation = accommodation
                    });


        private readonly IContractManagerContextService _contractManagerContext;
        private readonly IDocumentManagementService _documentManagementService;
        private readonly DirectContractsDbContext _dbContext;


        private class ContractAccommodationRelationAndAccommodation
        {
            public ContractAccommodationRelation? ContractAccommodationRelation { get; set; }
            public Accommodation? Accommodation { get; set; }
        }
    }
}