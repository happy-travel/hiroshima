using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.CancellationPolicies;
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.Data.Extensions;
using HappyTravel.Hiroshima.DirectManager.Infrastructure.Extensions;
using HappyTravel.Hiroshima.DirectManager.RequestValidators;
using Microsoft.EntityFrameworkCore;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public class CancellationPolicyManagementService : ICancellationPolicyManagementService
    {
        public CancellationPolicyManagementService(IContractManagerContextService contractManagerContext, DirectContractsDbContext dbContext)
        {
            _dbContext = dbContext;
            _contractManagerContext = contractManagerContext;
        }


        public Task<Result<List<Models.Responses.CancellationPolicy>>> Get(int contractId, int skip, int top, List<int> roomIds = null, List<int> seasonIds = null)
        {
            return _contractManagerContext.GetContractManager()
                .Map(contractManager => GetCancellationPolicies(contractId, contractManager.Id, skip, top, roomIds, seasonIds))
                .Map(Build);
        }


        public Task<Result<List<Models.Responses.CancellationPolicy>>> Add(int contractId, List<Models.Requests.CancellationPolicy> cancellationPolicies)
        {
            return ValidationHelper.Validate(cancellationPolicies, new CancellationPoliciesValidator())
                .Bind(() => _contractManagerContext.GetContractManager())
                .EnsureContractBelongsToContractManager(_dbContext, contractId)
                .Bind(async contractManager =>
                {
                    var (isSuccess, _, error) = await CheckIfSeasonIdsAndRoomIdsBelongToContract(contractManager.Id);
                    
                    return isSuccess ? Result.Success() : Result.Failure(error);
                })
                .Bind(() => AddCancellationPolicies(cancellationPolicies));
            
            
            async Task<Result> CheckIfSeasonIdsAndRoomIdsBelongToContract(int contractManagerId)
                => Result.Combine(await _dbContext.CheckIfSeasonsBelongToContract(contractId, cancellationPolicies.Select(rate => rate.SeasonId).ToList()),
                    await _dbContext.CheckIfRoomsBelongToContract(contractId, contractManagerId, cancellationPolicies.Select(rate => rate.RoomId).ToList()));
        }
        
        
        private async Task<Result<List<Models.Responses.CancellationPolicy>>> AddCancellationPolicies(List<Models.Requests.CancellationPolicy> cancellationPolicies)
        {
            var roomCancellationPolicies = CreateRoomCancellationPolicies(cancellationPolicies);
            _dbContext.RoomCancellationPolicies.AddRange(roomCancellationPolicies);
            await _dbContext.SaveChangesAsync();
            _dbContext.DetachEntries(roomCancellationPolicies);
            
            return Build(roomCancellationPolicies);
        }


        private List<RoomCancellationPolicy> CreateRoomCancellationPolicies(List<Models.Requests.CancellationPolicy> cancellationPolicies)
            => cancellationPolicies.Select(cancellationPolicy => new RoomCancellationPolicy
            {
                RoomId = cancellationPolicy.RoomId,
                SeasonId = cancellationPolicy.SeasonId,
                Policies = cancellationPolicy.Policies
            }).ToList();
            
        
        
        public async Task<Result> Remove(int contractId, List<int> cancellationPolicyIds)
        {
            return await _contractManagerContext.GetContractManager()
                .EnsureContractBelongsToContractManager(_dbContext, contractId)
                .Bind(contractManager => GetCancellationPoliciesToRemove(contractId, contractManager.Id, cancellationPolicyIds))
                .Tap(RemoveCancellationPolicies);
        }
        
        
        private async Task<Result<List<RoomCancellationPolicy>>> GetCancellationPoliciesToRemove(int contractId, int contractManagerId, List<int> cancellationPolicyIds)
        {
            var cancellationPolicies = await _dbContext.RoomCancellationPolicies.Where(cancellationPolicy => cancellationPolicyIds.Contains(cancellationPolicy.Id)).ToListAsync();
            if (cancellationPolicies == null || !cancellationPolicies.Any())
                return Result.Success(cancellationPolicies);

            var checkingResult = Result.Combine(await _dbContext.CheckIfSeasonsBelongToContract(contractId, cancellationPolicies.Select(cancellationPolicy => cancellationPolicy.SeasonId).ToList()),
                await _dbContext.CheckIfRoomsBelongToContract(contractId, contractManagerId, cancellationPolicies.Select(cancellationPolicy => cancellationPolicy.RoomId).ToList()));

            return checkingResult.IsFailure
                ? Result.Failure<List<RoomCancellationPolicy>>(checkingResult.Error)
                : Result.Success(cancellationPolicies);
        }
        
        
        private async Task<List<RoomCancellationPolicy>> GetCancellationPolicies(int contractId, int contractManagerId, int skip, int top, List<int> roomIds = null, List<int> seasonIds = null)
        {
            var contractedAccommodationIds = _dbContext.GetContractedAccommodations(contractId, contractManagerId)
                .Select(accommodation => accommodation.Id);
            
            var cancellationPoliciesAndRoomsAndSeasons = _dbContext.RoomCancellationPolicies
                .Join(_dbContext.Rooms, cancellationPolicy => cancellationPolicy.RoomId, room => room.Id, (cancellationPolicy, room) => new
                    {cancellationPolicy, room})
                .Join(_dbContext.Seasons, cancellationPolicyAndRoom => cancellationPolicyAndRoom.cancellationPolicy.SeasonId, season => season.Id, (cancellationPolicyAndRoom, season) => new
                    {cancellationPolicyAndRoom.cancellationPolicy, cancellationPolicyAndRoom.room, season})
                .Where(cancellationPolicyAndRoomAndSeason => contractedAccommodationIds.Contains(cancellationPolicyAndRoomAndSeason.room.AccommodationId))
                .Where(cancellationPolicyAndRoomAndSeason => cancellationPolicyAndRoomAndSeason.season.ContractId == contractId);
            
            if (roomIds != null && roomIds.Any())
                cancellationPoliciesAndRoomsAndSeasons = cancellationPoliciesAndRoomsAndSeasons.Where(rateAndRoomAndSeason => roomIds.Contains(rateAndRoomAndSeason.room.Id));
            
            if (seasonIds != null && seasonIds.Any())
                cancellationPoliciesAndRoomsAndSeasons = cancellationPoliciesAndRoomsAndSeasons.Where(rateAndRoomAndSeason => seasonIds.Contains(rateAndRoomAndSeason.season.Id));
            
            return await cancellationPoliciesAndRoomsAndSeasons.OrderBy(cancellationPolicyAndRoomAndSeason => cancellationPolicyAndRoomAndSeason.cancellationPolicy.Id).Skip(skip).Take(top)
                .Select(cancellationPolicyAndRoomAndSeason => cancellationPolicyAndRoomAndSeason.cancellationPolicy).Distinct().ToListAsync();
        }
        
        
        private async Task RemoveCancellationPolicies(List<RoomCancellationPolicy> cancellationPolicies)
        {
            if (!cancellationPolicies.Any())
                return;
            
            _dbContext.RoomCancellationPolicies.RemoveRange(cancellationPolicies);
            await _dbContext.SaveChangesAsync();
        }
        
        
        private List<Models.Responses.CancellationPolicy> Build(List<RoomCancellationPolicy> cancellationPolicies)
            => cancellationPolicies
                .Select(cancellationPolicy => new Models.Responses.CancellationPolicy(cancellationPolicy.Id,
                        cancellationPolicy.RoomId,
                        cancellationPolicy.SeasonId,
                        cancellationPolicy.Policies))
                .ToList();
        
        
        private readonly DirectContractsDbContext _dbContext;
        private readonly IContractManagerContextService _contractManagerContext;
    }
}