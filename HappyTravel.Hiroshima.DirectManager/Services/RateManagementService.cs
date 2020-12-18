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
using HappyTravel.Hiroshima.DirectManager.RequestValidators;
using Microsoft.EntityFrameworkCore;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public class RateManagementService : IRateManagementService
    {
        public RateManagementService(IManagerContextService managerContextService, IServiceSupplierContextService serviceSupplierContextService,
            DirectContractsDbContext dbContext)
        {
            _dbContext = dbContext;
            _managerContext = managerContextService;
            _serviceSupplierContext = serviceSupplierContextService;
        }


        public Task<Result<List<Models.Responses.Rate>>> Get(int contractId, int skip, int top, List<int> roomIds = null, List<int> seasonIds = null)
        {
            return _managerContext.GetServiceSupplier()
                .Map(serviceSupplier => GetRates(serviceSupplier.Id))
                .Map(Build);
            
            
            async Task<List<RoomRate>> GetRates(int serviceSupplierId)
            {
                var contractedAccommodationIds = _dbContext.GetContractedAccommodations(contractId, serviceSupplierId)
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
                .Bind(() => _managerContext.GetServiceSupplier())
                .Ensure(serviceSupplier => ratesRequest.Any(), "Request is empty")
                .Check(serviceSupplier => _serviceSupplierContext.EnsureContractBelongsToServiceSupplier(serviceSupplier, contractId))
                .Bind(serviceSupplier => CheckIfSeasonIdsAndRoomIdsBelongToContract(serviceSupplier.Id))
                .Bind(CheckIfAlreadyExists)
                .Map(() => Create(ratesRequest))
                .Map(AddRates)
                .Map(Build);


            async Task<Result> CheckIfAlreadyExists()
            {
                var seasonIdsFromRequest = ratesRequest.Select(rate => rate.SeasonId).ToList();
                var roomIdsFromRequest = ratesRequest.Select(rate => rate.RoomId).ToList();
                var roomTypesFromRequest = ratesRequest.Select(rate => rate.RoomType).ToList();
                var boardBasisFromRequest = ratesRequest.Select(rate => rate.BoardBasis).ToList();

                var existedRates = await _dbContext.RoomRates.Where(roomRate
                        => seasonIdsFromRequest.Contains(roomRate.SeasonId) && roomIdsFromRequest.Contains(roomRate.RoomId) &&
                        roomTypesFromRequest.Contains(roomRate.RoomType) && boardBasisFromRequest.Contains(roomRate.BoardBasis))
                    .ToListAsync();

                return !existedRates.Any() ? Result.Success() : Result.Failure(CreateError());


                string CreateError()
                    => "Existed rates: " + string.Join("; ",
                        existedRates.Select(rate
                            => $"{nameof(rate.RoomId)} '{rate.RoomId}' {nameof(rate.SeasonId)} '{rate.SeasonId}' {nameof(rate.RoomType)} '{rate.RoomType}' {nameof(rate.BoardBasis)} '{rate.BoardBasis}'"));
            }


            async Task<Result> CheckIfSeasonIdsAndRoomIdsBelongToContract(int serviceSupplierId)
                => Result.Combine(await _dbContext.CheckIfSeasonsBelongToContract(contractId, ratesRequest.Select(rate => rate.SeasonId).ToList()),
                    await _dbContext.CheckIfRoomsBelongToContract(contractId, serviceSupplierId, ratesRequest.Select(rate => rate.RoomId).ToList()));
        }


        public async Task<Result> Remove(int contractId, List<int> rateIds)
        {
            return await _managerContext.GetServiceSupplier()
                .Check(serviceSupplier => _serviceSupplierContext.EnsureContractBelongsToServiceSupplier(serviceSupplier, contractId))
                .Bind(serviceSupplier => GetRatesToRemove(contractId, serviceSupplier.Id, rateIds))
                .Tap(RemoveRates);
        }

      
        private async Task<Result<List<RoomRate>>> GetRatesToRemove(int contractId, int serviceSupplierId, List<int> rateIds)
        {
            var roomRates = await _dbContext.RoomRates.Where(rate => rateIds.Contains(rate.Id)).ToListAsync();
            if (roomRates == null || !roomRates.Any())
                return Result.Success(roomRates);

            var checkingResult = Result.Combine(await _dbContext.CheckIfSeasonsBelongToContract(contractId, roomRates.Select(roomRate => roomRate.SeasonId).ToList()),
                await _dbContext.CheckIfRoomsBelongToContract(contractId, serviceSupplierId, roomRates.Select(roomRate => roomRate.RoomId).ToList()));

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


        public Task<Result<Models.Responses.Rate>> Modify(int contractId, int rateId, Models.Requests.Rate rateRequest)
        {
            return ValidationHelper.Validate(rateRequest, new RateValidator())
                .Bind(() => _managerContext.GetServiceSupplier())
                .Check(serviceSupplier => _serviceSupplierContext.EnsureContractBelongsToServiceSupplier(serviceSupplier, contractId))
                .Bind(_ => Update());


            Task<Result<Models.Responses.Rate>> Update()
            {
                return GetRoomRate().Bind(UpdateDbEntry).Bind(UpdateDb).Map(Build);


                async Task<Result<RoomRate>> GetRoomRate()
                {
                    var roomRate = await _dbContext.RoomRates.Include(rr => rr.Season)
                        .SingleOrDefaultAsync(rr => rr.Id == rateId && rr.Season.ContractId == contractId);

                    return roomRate is null
                        ? Result.Failure<RoomRate>("Failed to retrieve the room rate") 
                        : Result.Success(roomRate);
                }


                Result<RoomRate> UpdateDbEntry(RoomRate roomRate)
                {
                    roomRate.RoomId = rateRequest.RoomId;
                    roomRate.SeasonId = rateRequest.SeasonId;
                    roomRate.Price = rateRequest.Price;
                    roomRate.Currency = rateRequest.Currency;
                    roomRate.BoardBasis = rateRequest.BoardBasis;
                    roomRate.MealPlan = rateRequest.MealPlan;
                    roomRate.RoomType = rateRequest.RoomType;
                    roomRate.Description = rateRequest.Description;

                    return roomRate;
                }


                async Task<Result<RoomRate>> UpdateDb(RoomRate roomRate)
                {
                    var entry = _dbContext.RoomRates.Update(roomRate);
                    await _dbContext.SaveChangesAsync();

                    _dbContext.DetachEntry(entry.Entity);

                    return entry.Entity;
                }
            }
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
                    Description = rate.Description
                })
                .ToList();


        private List<Models.Responses.Rate> Build(List<RoomRate> rates)
            => rates.Select(Build)
                .ToList();


        private Models.Responses.Rate Build(RoomRate rate)
            => new Models.Responses.Rate(rate.Id, rate.RoomId, rate.SeasonId, rate.Price, rate.Currency, rate.BoardBasis, rate.MealPlan, rate.RoomType,
                rate.Description);
        
        
        private readonly DirectContractsDbContext _dbContext;
        private readonly IManagerContextService _managerContext;
        private readonly IServiceSupplierContextService _serviceSupplierContext;
    }
}