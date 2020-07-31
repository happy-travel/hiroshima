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
using HappyTravel.Hiroshima.DirectContracts.Services.Management;
using HappyTravel.Hiroshima.DirectManager.RequestValidators;
using Microsoft.EntityFrameworkCore;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public class RateManagementService : IRateManagementService
    {
        public RateManagementService(IContractManagerContextService contractManagerContext, DirectContractsDbContext dbContext,
            IContractManagementRepository contractManagementRepository)
        {
            _dbContext = dbContext;
            _contractManagementRepository = contractManagementRepository;
            _contractManagerContext = contractManagerContext;
        }


        public Task<Result<List<Models.Responses.Rate>>> Get(int contractId, List<int> roomIds = null, List<int> seasonIds = null)
        {
            return _contractManagerContext.GetContractManager()
                .Map(contractManager => GetRates(contractId, contractManager.Id, roomIds, seasonIds))
                .Map(CreateResponse);
        }


        public Task<Result<List<Models.Responses.Rate>>> Add(int contractId, List<Models.Requests.Rate> rates)
        {
            return ValidationHelper.Validate(rates, new RateValidator())
                .Bind(() => _contractManagerContext.GetContractManager())
                .Ensure(contractManager => DoesContractBelongToContractManager(contractId, contractManager.Id),
                    $"Failed to get the contract by {nameof(contractId)} '{contractId}'")
                .Tap(async contractManager => Result.Combine(await CheckIfSeasonsBelongToContract(contractId, rates.Select(rate => rate.SeasonId).ToList()),
                    await CheckIfRoomIdsBelongToContractedAccommodation(contractId, contractManager.Id, rates.Select(rate => rate.RoomId).ToList())))
                .Bind(contractManager => AddRates(rates));
        }


        public Task<Result> Remove(int contractId, List<int> rateIds)
        {
            return _contractManagerContext.GetContractManager()
                .Ensure(contractManager => DoesContractBelongToContractManager(contractId, contractManager.Id),
                    $"Failed to get the contract by {nameof(contractId)} '{contractId}'")
                .Bind(contractManager => CheckAndGetRatesToRemove(contractId, contractManager.Id, rateIds))
                .Bind(RemoveRates);
        }


        private async Task<List<RoomRate>> GetRates(int contractId, int contractManagerId, List<int> roomIds = null, List<int> seasonIds = null)
        {
            var contractedAccommodationIds = _dbContext.Accommodations
                .Join(_dbContext.ContractAccommodationRelations, accommodation => accommodation.Id, relation => relation.AccommodationId,
                    (accommodation, relation) => new {accommodation, relation})
                .Join(_dbContext.Contracts, accommodationAndRelation => accommodationAndRelation.relation.ContractId, contract => contract.Id,
                    (accommodationAndRelation, contract) => new {accommodationAndRelation, contract})
                .Where(accommodationAndRelationAndContract => accommodationAndRelationAndContract.contract.ContractManagerId == contractManagerId &&
                    accommodationAndRelationAndContract.contract.Id == contractId)
                .Select(accommodationAndRelationAndContract => accommodationAndRelationAndContract.accommodationAndRelation.accommodation.Id);

            var ratesAndRoomsAndSeasons = _dbContext.RoomRates
                .Join(_dbContext.Rooms, roomRate => roomRate.RoomId, room => room.Id, (roomRate, room) => new
                    {roomRate, room})
                .Join(_dbContext.Seasons, rateAndRoom => rateAndRoom.roomRate.SeasonId, season => season.Id, (roomAndRate, season) => new
                    {roomAndRate.roomRate, roomAndRate.room, season})
                .Where(rateAndRoomAndSeason => contractedAccommodationIds.Contains(rateAndRoomAndSeason.room.AccommodationId))
                .Where(rateAndRoomAndSeason => rateAndRoomAndSeason.season.ContractId == contractId);
            
            if (roomIds != null && roomIds.Any())
                ratesAndRoomsAndSeasons = ratesAndRoomsAndSeasons.Where(rateAndRoomAndSeason => roomIds.Contains(rateAndRoomAndSeason.room.Id));

            if (seasonIds != null && seasonIds.Any())
                ratesAndRoomsAndSeasons = ratesAndRoomsAndSeasons.Where(rateAndRoomAndSeason => seasonIds.Contains(rateAndRoomAndSeason.season.Id));

            return await ratesAndRoomsAndSeasons.Select(i => i.roomRate).Distinct().ToListAsync();
        }


        private async Task<Result<List<RoomRate>>> CheckAndGetRatesToRemove(int contractId, int contractManagerId, List<int> rateIds)
        {
            var roomRates = await _dbContext.RoomRates.Where(rate => rateIds.Contains(rate.Id)).ToListAsync();
            if (roomRates == null || !roomRates.Any())
                return Result.Success(roomRates);

            var checkingResult = Result.Combine(await CheckIfSeasonsBelongToContract(contractId, roomRates.Select(roomRate => roomRate.SeasonId).ToList()),
                await CheckIfRoomIdsBelongToContractedAccommodation(contractId, contractManagerId, roomRates.Select(roomRate => roomRate.RoomId).ToList()));

            return checkingResult.IsFailure
                ? Result.Failure<List<RoomRate>>(checkingResult.Error)
                : Result.Success(roomRates);
        }


        private async Task<Result> RemoveRates(List<RoomRate> rates)
        {
            if (!rates.Any())
                return Result.Success();
            
            _dbContext.RoomRates.RemoveRange(rates);
            await _dbContext.SaveChangesAsync();
            
            _dbContext.DetachEntries(rates);
            
            return Result.Success();
        }


        private async Task<Result<List<Models.Responses.Rate>>> AddRates(List<Models.Requests.Rate> rates)
        {
            var newRates = CreateRates(rates);
            _dbContext.RoomRates.AddRange(newRates);
            await _dbContext.SaveChangesAsync();
            _dbContext.DetachEntries(newRates);
            return CreateResponse(newRates);
        }


        private List<RoomRate> CreateRates(List<Models.Requests.Rate> rates)
            => rates.Select(rate => new RoomRate
                {
                    RoomId = rate.RoomId,
                    SeasonId = rate.SeasonId,
                    BoardBasis = rate.BoardBasis,
                    MealPlan = rate.MealPlan,
                    Currency = rate.Currency,
                    Price = rate.Price,
                    Details = JsonDocumentUtilities.CreateJDocument(rate.Details)
                })
                .ToList();


        private List<Models.Responses.Rate> CreateResponse(List<RoomRate> rates)
            => rates.Select(rate => new Models.Responses.Rate(
                    rate.Id,
                    rate.RoomId,
                    rate.SeasonId,
                    rate.Price,
                    rate.BoardBasis,
                    rate.MealPlan,
                    rate.Details?.GetValue<MultiLanguage<string>>()))
                .ToList();


        private async Task<bool> DoesContractBelongToContractManager(int contractId, int contractManagerId)
            => await _contractManagementRepository.GetContract(contractId, contractManagerId) != null;


        private async Task<Result> CheckIfSeasonsBelongToContract(int contractId, List<int> seasonIds)
        {
            var availableSeasonIds =
                await _dbContext.Seasons.Where(s => seasonIds.Contains(s.Id) && s.ContractId == contractId).Select(s => s.Id).ToListAsync();

            var inappropriateSeasonIds = seasonIds.Except(availableSeasonIds).ToList();

            return inappropriateSeasonIds.Any() ? Result.Failure($"Inappropriate season ids: {string.Join(", ", inappropriateSeasonIds)}") : Result.Success();
        }


        private async Task<Result> CheckIfRoomIdsBelongToContractedAccommodation(int contractId, int contractManagerId, List<int> roomIds)
        {
            var availableRoomIds = await _dbContext.Rooms
                .Join(_dbContext.Accommodations, room => room.AccommodationId, accommodation => accommodation.Id, (room, accommodation) => new
                {
                    room,
                    accommodation
                })
                .Join(_dbContext.ContractAccommodationRelations, roomAndAccommodation=> roomAndAccommodation.accommodation.Id, relation => relation.AccommodationId, (roomAndAccommodation, relation) => new {roomAndAccommodation.accommodation, roomAndAccommodation.room, relation})
                .Join(_dbContext.Contracts, accommodationAndRoomAndRelation => accommodationAndRoomAndRelation.relation.ContractId , contract => contract.Id,
                    (accommodationAndRoomAndRelation, contract) => new
                    {
                        accommodationAndRoomAndRelation.accommodation,
                        accommodationAndRoomAndRelation.room,
                        accommodationAndRoomAndRelation.relation,
                        contract
                    })
                .Where(accommodationAndRoomAndRelationAndContract => accommodationAndRoomAndRelationAndContract.contract.Id == contractId &&
                    roomIds.Contains(accommodationAndRoomAndRelationAndContract.room.Id) &&
                    accommodationAndRoomAndRelationAndContract.contract.ContractManagerId == contractManagerId)
                .Select(accommodationAndRoomAndRelationAndContract => accommodationAndRoomAndRelationAndContract.room.Id)
                .ToListAsync();
                
            var inappropriateRoomIds = roomIds.Except(availableRoomIds).ToList();

            return inappropriateRoomIds.Any() 
                ? Result.Failure($"Inappropriate room ids: {string.Join(", ", inappropriateRoomIds)}") 
                : Result.Success();
        }


        private readonly DirectContractsDbContext _dbContext;
        private readonly IContractManagementRepository _contractManagementRepository;
        private readonly IContractManagerContextService _contractManagerContext;
    }
}