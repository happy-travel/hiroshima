using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
    public class RateManagementService : IRateManagementService
    {
        public RateManagementService(IContractManagerContextService contractManagerContext, DirectContractsDbContext dbContext)
        {
            _dbContext = dbContext;
            _contractManagerContext = contractManagerContext;
        }


        public Task<Result<List<Models.Responses.Rate>>> Get(int contractId, int skip, int top, List<int> roomIds = null, List<int> seasonIds = null)
        {
            return _contractManagerContext.GetContractManager()
                .Map(contractManager => GetRates(contractManager.Id))
                .Map(Build);
            
            
            async Task<List<RoomRate>> GetRates(int contractManagerId)
            {
                var contractedAccommodationIds = _dbContext.GetContractedAccommodations(contractId, contractManagerId)
                    .Select(accommodation => accommodation.Id);
            
                var ratesAndRoomsAndSeasons = _dbContext.RoomRates
                    .Join(_dbContext.Rooms, roomRate => roomRate.RoomId, room => room.Id, (roomRate, room) => new
                        {roomRate, room})
                    .Join(_dbContext.Seasons, rateAndRoom => rateAndRoom.roomRate.SeasonId, season => season.Id, (roomAndRate, season) => new
                        {roomAndRate.roomRate, roomAndRate.room, season})
                    .Where(rateAndRoomAndSeason => contractedAccommodationIds.Contains(rateAndRoomAndSeason.room.AccommodationId))
                    .Where(rateAndRoomAndSeason => rateAndRoomAndSeason.season.ContractId == contractId);

                if (roomIds != null && roomIds.Any())
                {
                    ratesAndRoomsAndSeasons = ratesAndRoomsAndSeasons.Where(rateAndRoomAndSeason => roomIds.Contains(rateAndRoomAndSeason.room.Id));
                }

                if (seasonIds != null && seasonIds.Any())
                {
                    ratesAndRoomsAndSeasons = ratesAndRoomsAndSeasons.Where(rateAndRoomAndSeason => seasonIds.Contains(rateAndRoomAndSeason.season.Id));
                }
                
                return await ratesAndRoomsAndSeasons.OrderBy(rateAndRoomAndSeason => rateAndRoomAndSeason.roomRate.Id)
                    .Skip(skip).Take(top).Select(i => i.roomRate).Distinct().ToListAsync();
            }
        }


        public async Task<Result<List<Models.Responses.Rate>>> Add(int contractId, List<Models.Requests.Rate> ratesRequest)
        {
            return await ValidationHelper.Validate(ratesRequest, new RateValidator())
                .Bind(() => _contractManagerContext.GetContractManager())
                .Ensure(contractManager => ratesRequest.Any(), "Request is empty")
                .EnsureContractBelongsToContractManager(_dbContext, contractId)
                .Bind(contractManager => CheckIfSeasonIdsAndRoomIdsBelongToContract(contractManager.Id))
                .Bind(CheckIfRatesAlreadyExist)
                .Map(() => Create(ratesRequest))
                .Map(AddRates)
                .Map(Build);


            async Task<Result> CheckIfRatesAlreadyExist()
            {
                var seasonIdsFromRequest = ratesRequest.Select(rate => rate.SeasonId).ToList();
                var roomIdsFromRequest = ratesRequest.Select(rate => rate.RoomId).ToList();
                var roomTypesFromRequest = ratesRequest.Select(rate => rate.RoomType).ToList();
                var boardBasisFromRequest = ratesRequest.Select(rate => rate.BoardBasis).ToList();

                var existedRates = await _dbContext.RoomRates.Where(roomRate
                        => seasonIdsFromRequest.Contains(roomRate.SeasonId) && roomIdsFromRequest.Contains(roomRate.RoomId) &&
                        roomTypesFromRequest.Contains(roomRate.RoomType) && boardBasisFromRequest.Contains(roomRate.BoardBasis))
                    .ToListAsync();

                return !existedRates.Any() ? Result.Success() : Result.Failure(CreateRateError());


                string CreateRateError()
                    => "Existed rates: " + string.Join("; ",
                        existedRates.Select(rate
                            => $"{nameof(rate.RoomId)} '{rate.RoomId}' {nameof(rate.SeasonId)} '{rate.SeasonId}' {nameof(rate.RoomType)} '{rate.RoomType}' {nameof(rate.BoardBasis)} '{rate.BoardBasis}'"));
            }


            async Task<Result> CheckIfSeasonIdsAndRoomIdsBelongToContract(int contractManagerId)
                => Result.Combine(await _dbContext.CheckIfSeasonsBelongToContract(contractId, ratesRequest.Select(rate => rate.SeasonId).ToList()),
                    await _dbContext.CheckIfRoomsBelongToContract(contractId, contractManagerId, ratesRequest.Select(rate => rate.RoomId).ToList()));
        }


        public async Task<Result> Remove(int contractId, List<int> rateIds)
        {
            return await _contractManagerContext.GetContractManager()
                .EnsureContractBelongsToContractManager(_dbContext, contractId)
                .Bind(contractManager => GetRatesToRemove(contractId, contractManager.Id, rateIds))
                .Tap(RemoveRates);
        }

      
        private async Task<Result<List<RoomRate>>> GetRatesToRemove(int contractId, int contractManagerId, List<int> rateIds)
        {
            var roomRates = await _dbContext.RoomRates.Where(rate => rateIds.Contains(rate.Id)).ToListAsync();
            if (roomRates == null || !roomRates.Any())
                return Result.Success(roomRates);

            var checkingResult = Result.Combine(await _dbContext.CheckIfSeasonsBelongToContract(contractId, roomRates.Select(roomRate => roomRate.SeasonId).ToList()),
                await _dbContext.CheckIfRoomsBelongToContract(contractId, contractManagerId, roomRates.Select(roomRate => roomRate.RoomId).ToList()));

            return checkingResult.IsFailure
                ? Result.Failure<List<RoomRate>>(checkingResult.Error)
                : Result.Success(roomRates);
        }


        private async Task RemoveRates(List<RoomRate> rates)
        {
            if (!rates.Any())
                return;
            
            _dbContext.RoomRates.RemoveRange(rates);
            await _dbContext.SaveChangesAsync();
        }


        private async Task<List<RoomRate>> AddRates(List<RoomRate> dbRates)
        {
            _dbContext.RoomRates.AddRange(dbRates);
            await _dbContext.SaveChangesAsync();
            _dbContext.DetachEntries(dbRates);
            return dbRates;
        }


        private List<RoomRate> Create(List<Models.Requests.Rate> rates)
            => rates.Select(rate => new RoomRate
                {
                    RoomId = rate.RoomId,
                    SeasonId = rate.SeasonId,
                    BoardBasis = rate.BoardBasis,
                    MealPlan = rate.MealPlan,
                    Currency = rate.Currency,
                    Price = rate.Price,
                    RoomType = rate.RoomType,
                    Remarks = JsonDocumentUtilities.CreateJDocument(rate.Details)
                })
                .ToList();


        private List<Models.Responses.Rate> Build(List<RoomRate> rates)
            => rates.Select(rate => new Models.Responses.Rate(
                    rate.Id,
                    rate.RoomId,
                    rate.SeasonId,
                    rate.Price,
                    rate.Currency,
                    rate.BoardBasis,
                    rate.MealPlan,
                    rate.RoomType,
                    rate.Remarks.GetValue<MultiLanguage<string>>()))
                .ToList();


        private readonly DirectContractsDbContext _dbContext;
        private readonly IContractManagerContextService _contractManagerContext;
    }
}