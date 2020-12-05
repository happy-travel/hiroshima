using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Common.Infrastructure.Extensions;
using HappyTravel.Hiroshima.Common.Infrastructure.Utilities;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms;
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.Data.Extensions;
using HappyTravel.Hiroshima.DirectManager.Infrastructure.Extensions;
using HappyTravel.Hiroshima.DirectManager.RequestValidators;
using Microsoft.EntityFrameworkCore;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public class PromotionalOfferManagementService: IPromotionalOfferManagementService
    {
        public PromotionalOfferManagementService(IContractManagerContextService contractManagerContext, DirectContractsDbContext dbContext)
        {
            _contractManagerContext = contractManagerContext;
            _dbContext = dbContext;
        }


        public Task<Result<List<Models.Responses.PromotionalOffer>>> Add(int contractId, List<Models.Requests.PromotionalOffer> promotionalOffers)
        {
            return ValidationHelper.Validate(promotionalOffers, new PromotionalOfferValidator())
                .Bind(() => _contractManagerContext.GetContractManager())
                .EnsureContractBelongsToContractManager(_dbContext, contractId)
                .Bind(contractManager
                    => _dbContext.CheckIfRoomsBelongToContract(contractId, contractManager.Id, promotionalOffers.Select(offer => offer.RoomId).ToList()))
                .Map(AddPromotionalOffers)
                .Map(Build);
            
            
            async Task<List<RoomPromotionalOffer>> AddPromotionalOffers()
            {
                var newPromotionalOffers = Create(contractId, promotionalOffers);
                _dbContext.PromotionalOffers.AddRange(newPromotionalOffers);
                await _dbContext.SaveChangesAsync();
                _dbContext.DetachEntries(newPromotionalOffers);
            
                return newPromotionalOffers;
            }
        }

        
        public Task<Result<List<Models.Responses.PromotionalOffer>>> Get(int contractId, int skip, int top, List<int> roomIds, DateTime? validFrom = null, DateTime? validTo = null)
        {
            return _contractManagerContext.GetContractManager()
                .EnsureContractBelongsToContractManager(_dbContext, contractId)
                .Bind(contractManager => _dbContext.CheckIfRoomsBelongToContract(contractId, contractManager.Id, roomIds))
                .Map(GetPromotionalOffers)
                .Map(Build);
            
            
            async Task<List<RoomPromotionalOffer>> GetPromotionalOffers()
            {
                var promotionalOffers = _dbContext.PromotionalOffers.Where(offer => offer.ContractId == contractId);

                if (roomIds.Any())
                    promotionalOffers = promotionalOffers.Where(offer => roomIds.Contains(offer.RoomId));
            
                if (validFrom != null)
                    promotionalOffers = promotionalOffers.Where(offer => validFrom.Value <= offer.ValidFromDate);
            
                if (validTo != null)
                    promotionalOffers = promotionalOffers.Where(offer =>  offer.ValidToDate <= validTo.Value);

                return await promotionalOffers.OrderBy(offer => offer.Id).Skip(skip).Take(top)
                    .ToListAsync();
            }
        }
        

        public async Task<Result> Remove(int contractId, List<int> promotionalOfferIds)
        {
            return await _contractManagerContext.GetContractManager()
                .EnsureContractBelongsToContractManager(_dbContext, contractId)
                .Bind(contractManager => GetPromotionalOffersToRemove(contractManager.Id))
                .Tap(RemovePromotionalOffers);
            
            
            async Task<Result<List<RoomPromotionalOffer>>> GetPromotionalOffersToRemove(int contractManagerId)
            {
                var promotionalOffers = await _dbContext.PromotionalOffers.Where(offer => promotionalOfferIds.Contains(offer.Id) && offer.ContractId == contractId).ToListAsync();
                if (promotionalOffers == null || !promotionalOffers.Any())
                    return Result.Success(promotionalOffers);

                var checkingResult = await CheckIfRoomsBelongToContract();
                
                return checkingResult.IsFailure
                    ? Result.Failure<List<RoomPromotionalOffer>>(checkingResult.Error)
                    : Result.Success(promotionalOffers);
                
                
                Task<Result> CheckIfRoomsBelongToContract() 
                    => _dbContext.CheckIfRoomsBelongToContract(contractId, contractManagerId, promotionalOffers.Select(offer => offer.RoomId).ToList());
            }
            
            
            async Task RemovePromotionalOffers(List<RoomPromotionalOffer> promotionalOffers)
            {
                if (!promotionalOffers.Any())
                    return;

                _dbContext.PromotionalOffers.RemoveRange(promotionalOffers);
            
                await _dbContext.SaveChangesAsync();
            }
        }


        public Task<Result<List<Models.Responses.PromotionalOfferStopSalePeriod>>> AddStopSalePeriods(int contractId, List<Models.Requests.PromotionalOfferStopSale> stopSalePeriodsRequest)
        {
            return ValidationHelper.Validate(stopSalePeriodsRequest, new PromotionalOfferStopSaleValidator())
                .Bind(() => _contractManagerContext.GetContractManager())
                .EnsureContractBelongsToContractManager(_dbContext, contractId)
                .Map(contractManager
                    => _dbContext.CheckIfRoomsBelongToContract(contractId, contractManager.Id, stopSalePeriodsRequest.Select(offer => offer.RoomId).ToList()))
                .Tap(_ => RemovePrevious())
                .Map(_ => Create(contractId, stopSalePeriodsRequest))
                .Map(Add)
                .Map(Build);


            async Task RemovePrevious()
            {
                var roomIds = stopSalePeriodsRequest.Select(stopSale => stopSale.RoomId).ToList();
                var previousStopSales = await _dbContext.PromotionalOfferStopSales.Where(stopSale => stopSale.ContractId == contractId && roomIds.Contains(stopSale.RoomId)).ToListAsync();
                
                _dbContext.PromotionalOfferStopSales.RemoveRange(previousStopSales);
                
                await _dbContext.SaveChangesAsync();
            }

            async Task<List<PromotionalOfferStopSale>> Add(List<PromotionalOfferStopSale> stopSalePeriods)
            {
                _dbContext.PromotionalOfferStopSales.AddRange(stopSalePeriods);
                await _dbContext.SaveChangesAsync();
                
                _dbContext.DetachEntries(stopSalePeriods);
                return stopSalePeriods;
            }
        }


        public Task<Result<List<Models.Responses.PromotionalOfferStopSalePeriod>>> GetStopSalePeriods(int contractId, int skip, int top, List<int> roomIds,
            DateTime? fromDate, DateTime? toDate)
        {
            return _contractManagerContext.GetContractManager()
                .EnsureContractBelongsToContractManager(_dbContext, contractId)
                .Bind(contractManager => _dbContext.CheckIfRoomsBelongToContract(contractId, contractManager.Id, roomIds))
                .Map(GetStopSalePeriods)
                .Map(Build);


            async Task<List<PromotionalOfferStopSale>> GetStopSalePeriods()
            {
                var stopSalePeriods = _dbContext.PromotionalOfferStopSales.Where(stopSale => stopSale.ContractId == contractId);

                if (roomIds.Any())
                    stopSalePeriods = stopSalePeriods.Where(stopSale => roomIds.Contains(stopSale.RoomId));
            
                if (fromDate != null)
                    stopSalePeriods = stopSalePeriods.Where(stopSale => fromDate.Value <= stopSale.FromDate);
            
                if (toDate != null)
                    stopSalePeriods = stopSalePeriods.Where(stopSale =>  stopSale.ToDate <= toDate.Value);

                return await stopSalePeriods.OrderBy(offer => offer.Id).Skip(skip).Take(top)
                    .ToListAsync();
            }
        }


        public async Task<Result> RemoveStopSalePeriods(int contractId, List<int> stopSalePeriodIds)
        {
            return await _contractManagerContext.GetContractManager()
                .EnsureContractBelongsToContractManager(_dbContext, contractId)
                .Bind(contractManager => GetStopSalePeriodsToRemove(contractManager.Id))
                .Tap(RemoveStopSalePeriods);

            
            async Task<Result<List<PromotionalOfferStopSale>>> GetStopSalePeriodsToRemove(int contractManagerId)
            {
                var stopSales = await _dbContext.PromotionalOfferStopSales.Where(stopSale => stopSalePeriodIds.Contains(stopSale.Id) && stopSale.ContractId == contractId).ToListAsync();
                if (stopSales == null || !stopSales.Any())
                    return Result.Success(stopSales);

                var checkingResult = await CheckIfRoomsBelongToContract();
            
                return checkingResult.IsFailure
                    ? Result.Failure<List<PromotionalOfferStopSale>>(checkingResult.Error)
                    : Result.Success(stopSales);

                Task<Result> CheckIfRoomsBelongToContract() => 
                    _dbContext.CheckIfRoomsBelongToContract(contractId, contractManagerId, stopSales.Select(offer => offer.RoomId).ToList());
            }
            
            
            async Task RemoveStopSalePeriods(List<PromotionalOfferStopSale> stopSales)
            {
                if (!stopSales.Any())
                    return;

                _dbContext.PromotionalOfferStopSales.RemoveRange(stopSales);
            
                await _dbContext.SaveChangesAsync();
            }
        }
      
        
        private List<RoomPromotionalOffer> Create(int contractId, List<Models.Requests.PromotionalOffer> promotionalOffers)
            => promotionalOffers.Select(offer => new RoomPromotionalOffer
            {
                RoomId = offer.RoomId,
                ContractId = contractId,
                BookByDate = offer.BookByDate,
                ValidFromDate = offer.ValidFrom,
                ValidToDate = offer.ValidTo,
                DiscountPercent = offer.DiscountPercent,
                Description = offer.Description,
                BookingCode = offer.BookingCode
            }).ToList();


        private Models.Responses.PromotionalOffer Build(RoomPromotionalOffer promotionalOffer)
            => new Models.Responses.PromotionalOffer(promotionalOffer.Id, promotionalOffer.ContractId, promotionalOffer.RoomId, promotionalOffer.BookByDate,
                promotionalOffer.ValidFromDate,
                promotionalOffer.ValidToDate, promotionalOffer.DiscountPercent, promotionalOffer.BookingCode,
                promotionalOffer.Description);
        
        
        private List<Models.Responses.PromotionalOffer> Build(List<RoomPromotionalOffer> promotionalOffers)
            => promotionalOffers.Select(Build).ToList();


        private Models.Responses.PromotionalOfferStopSalePeriod Build(PromotionalOfferStopSale stopSalePeriod)
            => new Models.Responses.PromotionalOfferStopSalePeriod(stopSalePeriod.Id, stopSalePeriod.RoomId, stopSalePeriod.FromDate, stopSalePeriod.ToDate, stopSalePeriod.ContractId);


        private List<Models.Responses.PromotionalOfferStopSalePeriod> Build(List<PromotionalOfferStopSale> stopSalePeriods)
            => stopSalePeriods.Select(Build).ToList();
        
        
        private PromotionalOfferStopSale Create(int contractId, Models.Requests.PromotionalOfferStopSale stopSalePeriod)
            => new PromotionalOfferStopSale
            {
                RoomId = stopSalePeriod.RoomId,
                FromDate = stopSalePeriod.FromDate,
                ToDate = stopSalePeriod.ToDate,
                ContractId = contractId
            };


        private List<PromotionalOfferStopSale> Create(int contractId, List<Models.Requests.PromotionalOfferStopSale> stopSalePeriods)
            => stopSalePeriods.Select(stopSalePeriod => Create(contractId, stopSalePeriod)).ToList();
        
        
        private readonly IContractManagerContextService _contractManagerContext;
        private readonly DirectContractsDbContext _dbContext;
    }
}