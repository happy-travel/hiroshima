using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Common.Models.Accommodations;
using HappyTravel.Hiroshima.Common.Models.Seasons;
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
        public ContractManagementService(IManagerContextService managerContextService, IDocumentManagementService documentManagementService,
            DirectContractsDbContext dbContext)
        {
            _managerContext = managerContextService;
            _documentManagementService = documentManagementService;
            _dbContext = dbContext;
        }


        public Task<Result<Models.Responses.Contract>> Get(int contractId)
        {
            return _managerContext.GetServiceSupplier()
                .Bind(serviceSupplier => Get(serviceSupplier.Id));


            async Task<Result<Models.Responses.Contract>> Get(int serviceSupplierId)
            {
                var contract = await GetContractWithDocuments(contractId, serviceSupplierId);

                if (contract is null)
                    return Result.Failure<Models.Responses.Contract>($"Failed to get the contract with {nameof(contractId)} '{contractId}'");

                var accommodationId = (await GetRelatedAccommodations(contractId, serviceSupplierId)).Single().Id;

                return Build(contract, accommodationId);
            }
        }


        public Task<Result<List<Models.Responses.Contract>>> GetContracts(int skip, int top)
        {
            return _managerContext.GetServiceSupplier()
                .Map(Get);


            async Task<List<Models.Responses.Contract>> Get(ServiceSupplier serviceSupplier)
            {
                var contracts = await _dbContext.Contracts
                        .Where(contract => contract.ServiceSupplierId == serviceSupplier.Id).OrderBy(contract => contract.Id)
                        .Skip(skip)
                        .Take(top)
                        .ToListAsync();

                    if (!contracts.Any())
                        return new List<Models.Responses.Contract>();

                    var contractIds = contracts.Select(contract => contract.Id).ToList();
                    var contractsAccommodationRelations =
                        (await GetContractRelations(serviceSupplier.Id, contractIds)).ToDictionary(relation => relation.ContractId);

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
            return _managerContext.GetServiceSupplier()
                .EnsureAccommodationBelongsToCompany(_dbContext, contract.AccommodationId)
                .Bind(serviceSupplier =>
                {
                    var validationResult = ValidationHelper.Validate(contract, new ContractValidator());
                    
                    return validationResult.IsFailure 
                        ? Result.Failure<ServiceSupplier>(validationResult.Error) 
                        : Result.Success(serviceSupplier);
                })
                .Map(serviceSupplier => Create(serviceSupplier.Id, contract))
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
            return await _managerContext.GetServiceSupplier()
                .EnsureContractBelongsToCompany(_dbContext, contractId)
                .EnsureAccommodationBelongsToCompany(_dbContext, contract.AccommodationId)
                .Bind(serviceSupplier =>
                {
                    var (_, failure, error) = ValidationHelper.Validate(contract, new ContractValidator());

                    return failure ? Result.Failure<ServiceSupplier>(error) : Result.Success(serviceSupplier);
                })
                .Map(serviceSupplier => Create(serviceSupplier.Id, contract))
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
            return await _managerContext.GetServiceSupplier()
                .EnsureContractBelongsToCompany(_dbContext, contractId)
                .Tap(serviceSupplier => RemoveContractDocuments(serviceSupplier.Id))
                .Tap(async serviceSupplier => await RemoveContract(serviceSupplier.Id));

            async Task<Result> RemoveContractDocuments(int serviceSupplierId)
            {
                return await _documentManagementService.RemoveAll(serviceSupplierId, contractId);
            }

            async Task RemoveContract(int serviceSupplierId)
            {
                var contract = await _dbContext.Contracts.SingleOrDefaultAsync(c => c.Id == contractId && c.ServiceSupplierId == serviceSupplierId);
                if (contract is null)
                    return;

                await DeletePromotionalOffers();

                await DeletePromotionalOfferStopSales();

                await DeleteRoomAvailabilityRestrictions();

                await DeleteSeasonsAndDependentEntities();

                await DeleteContractAccommodationRelations();

                _dbContext.Contracts.Remove(contract);
                
                await _dbContext.SaveChangesAsync();
            }
            
            async Task DeleteContractAccommodationRelations()
            {
                var relations = await _dbContext.ContractAccommodationRelations
                    .Where(relation => relation.ContractId == contractId)
                    .ToListAsync();
                
                if (relations.Any())
                    _dbContext.ContractAccommodationRelations.RemoveRange(relations);
            }

            async Task DeletePromotionalOffers()
            {
                var offers = await _dbContext.PromotionalOffers
                    .Where(promotionalOffer => promotionalOffer.ContractId == contractId)
                    .ToListAsync();

                if (offers.Any())
                    _dbContext.PromotionalOffers.RemoveRange(offers);
            }

            async Task DeletePromotionalOfferStopSales()
            {
                var offers = await _dbContext.PromotionalOfferStopSales
                    .Where(promotionalOfferStopSale => promotionalOfferStopSale.ContractId == contractId)
                    .ToListAsync();

                if (offers.Any())
                    _dbContext.PromotionalOfferStopSales.RemoveRange(offers);
            }

            async Task DeleteRoomAvailabilityRestrictions()
            {
                var restrictions = await _dbContext.RoomAvailabilityRestrictions
                    .Where(roomAvailabilityRestriction => roomAvailabilityRestriction.ContractId == contractId)
                    .ToListAsync();

                if (restrictions.Any())
                    _dbContext.RoomAvailabilityRestrictions.RemoveRange(restrictions);
            }

            async Task DeleteSeasonsAndDependentEntities()
            {
                var seasons = await _dbContext.Seasons
                    .Where(season => season.ContractId == contractId)
                    .ToListAsync();

                if (seasons.Any())
                {
                    foreach (Season season in seasons)
                    {
                        await DeleteRoomRates(season.Id);

                        await DeleteRoomCancellationPolicies(season.Id);

                        await DeleteSeasonRanges(season.Id);
                    }

                    _dbContext.Seasons.RemoveRange(seasons);
                }
            }

            async Task DeleteRoomRates(int seasonId)
            {
                var roomRates = await _dbContext.RoomRates
                    .Where(roomRate => roomRate.SeasonId == seasonId)
                    .ToListAsync();

                if (roomRates.Any())
                    _dbContext.RoomRates.RemoveRange(roomRates);
            }

            async Task DeleteRoomCancellationPolicies(int seasonId)
            {
                var cancellationPolicies = await _dbContext.RoomCancellationPolicies
                    .Where(cancellationPolicy => cancellationPolicy.SeasonId == seasonId)
                    .ToListAsync();

                if (cancellationPolicies.Any())
                    _dbContext.RoomCancellationPolicies.RemoveRange(cancellationPolicies);
            }

            async Task DeleteSeasonRanges(int seasonId)
            {
                var seasonRanges = await _dbContext.SeasonRanges
                    .Where(seasonRange => seasonRange.SeasonId == seasonId)
                    .ToListAsync();

                if (seasonRanges.Any())
                {
                    foreach (SeasonRange seasonRange in seasonRanges)
                    {
                        await DeleteRoomAllocationRequirements(seasonRange.Id);
                    }

                    _dbContext.SeasonRanges.RemoveRange(seasonRanges);
                }
            }

            async Task DeleteRoomAllocationRequirements(int seasonRangeId)
            {
                var allocationRequirements = await _dbContext.RoomAllocationRequirements
                    .Where(allocationRequirement => allocationRequirement.SeasonRangeId == seasonRangeId)
                    .ToListAsync();

                if (allocationRequirements.Any())
                    _dbContext.RoomAllocationRequirements.RemoveRange(allocationRequirements);
            }
        }


        private Contract Create(int serviceSupplierId, Models.Requests.Contract contract)
            => new Contract
            {
                Name = contract.Name,
                Description = contract.Description,
                ValidFrom = contract.ValidFrom.Date,
                ValidTo = contract.ValidTo.Date,
                Modified = DateTime.UtcNow,
                ServiceSupplierId = serviceSupplierId
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


        private async Task<Contract> GetContractWithDocuments(int contractId, int serviceSupplierId)
        {
            var contract = await _dbContext.Contracts.SingleOrDefaultAsync(c => c.ServiceSupplierId == serviceSupplierId && c.Id == contractId);
            if (contract == null)
                return contract;

            contract.Documents = await _dbContext.Documents.Where(d => d.ServiceSupplierId == serviceSupplierId && d.ContractId == contractId).ToListAsync();

            return contract;
        }


        private async Task<List<Accommodation>> GetRelatedAccommodations(int contractId, int serviceSupplierId) =>
            (await JoinContractAccommodationRelationAndAccommodation()
                .Where(contractAccommodationRelationAndAccommodation =>
                    contractAccommodationRelationAndAccommodation.Accommodation!.ServiceSupplierId == serviceSupplierId &&
                    contractAccommodationRelationAndAccommodation.ContractAccommodationRelation!.ContractId ==
                    contractId)
                .Select(contractAccommodationRelationAndAccommodation => contractAccommodationRelationAndAccommodation.Accommodation)
                .ToListAsync())!;


        private async Task<List<ContractAccommodationRelation>> GetContractRelations(int serviceSupplierId, List<int> contractIds)
            => (await JoinContractAccommodationRelationAndAccommodation()
                .Where(contractAccommodationRelationAndAccommodation =>
                    contractAccommodationRelationAndAccommodation.Accommodation!.ServiceSupplierId == serviceSupplierId &&
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


        private readonly IManagerContextService _managerContext;
        private readonly IDocumentManagementService _documentManagementService;
        private readonly DirectContractsDbContext _dbContext;


        private class ContractAccommodationRelationAndAccommodation
        {
            public ContractAccommodationRelation? ContractAccommodationRelation { get; set; }
            public Accommodation? Accommodation { get; set; }
        }
    }
}