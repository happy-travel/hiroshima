using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Common.Infrastructure.Extensions;
using HappyTravel.Hiroshima.Common.Infrastructure.Utilities;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.Data.Extensions;
using HappyTravel.Hiroshima.Data.Models.Rooms;
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


        public Task<Result<List<Models.Responses.PromotionalOffer>>> Get(int contractId, List<int> roomIds, DateTime? validFrom = null, DateTime? validTo = null)
            => _contractManagerContext.GetContractManager()
                .Ensure(contractManager => _dbContext.DoesContractBelongToContractManager(contractId, contractManager.Id),
                    $"Failed to get the contract by {nameof(contractId)} '{contractId}'")
                .Bind(contractManager => _dbContext.CheckIfRoomsBelongToContract(contractId, contractManager.Id, roomIds))
                .Map(() => GetPromotionalOffers(contractId, roomIds, validFrom, validTo))
                .Map(CreateResponse);


        public Task<Result<List<Models.Responses.PromotionalOffer>>> Add(int contractId, List<Models.Requests.PromotionalOffer> promotionalOffers)
            => ValidationHelper.Validate(promotionalOffers, new PromotionalOfferValidator())
                .Bind(() => _contractManagerContext.GetContractManager())
                .Ensure(contractManager => _dbContext.DoesContractBelongToContractManager(contractId, contractManager.Id),
                    $"Failed to get the contract by {nameof(contractId)} '{contractId}'")
                .Bind(contractManager
                    => _dbContext.CheckIfRoomsBelongToContract(contractId, contractManager.Id, promotionalOffers.Select(offer => offer.RoomId).ToList()))
                .Map(() => AddPromotionalOffers(contractId, promotionalOffers));


        public Task<Result> Remove(int contractId, List<int> promotionalOfferIds)
            => _contractManagerContext.GetContractManager()
                .Ensure(contractManager => _dbContext.DoesContractBelongToContractManager(contractId, contractManager.Id),
                    $"Failed to get the contract by {nameof(contractId)} '{contractId}'")
                .Bind(contractManager => CheckAndGetPromotionalOffersToRemove(contractId, contractManager.Id, promotionalOfferIds))
                .Tap(RemovePromotionalOffers)
                .Finally(result => result.IsSuccess ? Result.Success() : Result.Failure(result.Error));


        private async Task RemovePromotionalOffers(List<RoomPromotionalOffer> promotionalOffers)
        {
            if (!promotionalOffers.Any())
                return;

            _dbContext.RoomPromotionalOffers.RemoveRange(promotionalOffers);
            await _dbContext.SaveChangesAsync();

            _dbContext.DetachEntries(promotionalOffers);
        }
        
        
        private async Task<Result<List<RoomPromotionalOffer>>> CheckAndGetPromotionalOffersToRemove(int contractId, int contractManagerId,
            List<int> promotionalOfferIds)
        {
            var promotionalOffers = await _dbContext.RoomPromotionalOffers.Where(offer => promotionalOfferIds.Contains(offer.Id) && offer.ContractId == contractId).ToListAsync();
            if (promotionalOffers == null || !promotionalOffers.Any())
                return Result.Success(promotionalOffers);

            var checkingResult = await _dbContext.CheckIfRoomsBelongToContract(contractId, contractManagerId, promotionalOffers.Select(offer => offer.RoomId).ToList());
            
            return checkingResult.IsFailure
                ? Result.Failure<List<RoomPromotionalOffer>>(checkingResult.Error)
                : Result.Success(promotionalOffers);
        }
        
        
        private async Task<List<Models.Responses.PromotionalOffer>> AddPromotionalOffers(int contractId, List<Models.Requests.PromotionalOffer> promotionalOffers)
        {
            var newPromotionalOffers = CreateRoomPromotionalOffers(contractId, promotionalOffers);
            _dbContext.RoomPromotionalOffers.AddRange(newPromotionalOffers);
            await _dbContext.SaveChangesAsync();
            
            _dbContext.DetachEntries(newPromotionalOffers);
            
            return CreateResponse(newPromotionalOffers);
        }


        private List<RoomPromotionalOffer> CreateRoomPromotionalOffers(int contractId, List<Models.Requests.PromotionalOffer> promotionalOffers)
            => promotionalOffers.Select(offer => new RoomPromotionalOffer
            {
                RoomId = offer.RoomId,
                ContractId = contractId,
                BookByDate = offer.BookByDate,
                ValidFromDate = offer.ValidFrom,
                ValidToDate = offer.ValidTo,
                DiscountPercent = offer.DiscountPercent,
                Details = JsonDocumentUtilities.CreateJDocument(offer.Details),
                BookingCode = offer.BookingCode
            }).ToList();
        
        
        private List<Models.Responses.PromotionalOffer> CreateResponse(List<RoomPromotionalOffer> promotionalOffers)
            => promotionalOffers.Select(offer => new Models.Responses.PromotionalOffer(offer.Id, offer.ContractId, offer.RoomId, offer.BookByDate, offer.ValidFromDate,
                    offer.ValidToDate, offer.DiscountPercent, offer.BookingCode, offer.Details.GetValue<MultiLanguage<string>>()))
                .ToList();
        
        
        private async Task<List<RoomPromotionalOffer>> GetPromotionalOffers(int contractId, List<int> roomIds, DateTime? validFrom = null, DateTime? validTo = null)
        {
            var promotionalOffers = _dbContext.RoomPromotionalOffers.Where(offer => offer.ContractId == contractId);

            if (roomIds.Any())
                promotionalOffers = promotionalOffers.Where(offer => roomIds.Contains(offer.RoomId));
            
            if (validFrom != null)
                promotionalOffers = promotionalOffers.Where(offer => validFrom.Value <= offer.ValidFromDate);
            
            if (validTo != null)
                promotionalOffers = promotionalOffers.Where(offer =>  offer.ValidToDate <= validTo.Value);

            return await promotionalOffers.ToListAsync();
        }
        
        
        private readonly IContractManagerContextService _contractManagerContext;
        private readonly DirectContractsDbContext _dbContext;
    }
}