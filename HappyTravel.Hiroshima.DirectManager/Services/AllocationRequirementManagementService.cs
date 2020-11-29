using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms;
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.Data.Extensions;
using HappyTravel.Hiroshima.DirectManager.Infrastructure.Extensions;
using HappyTravel.Hiroshima.DirectManager.RequestValidators;
using Microsoft.EntityFrameworkCore;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public class AllocationRequirementManagementService : IAllocationRequirementManagementService
    {
        public AllocationRequirementManagementService(DirectContractsDbContext dbContext, IContractManagerContextService managerContextService)
        {
            _managerContext = managerContextService;
            _dbContext = dbContext;
        }


        public Task<Result<List<Models.Responses.AllocationRequirement>>> Add(int contractId, List<Models.Requests.AllocationRequirement> allocationRequirements)
        {
            return Validate(allocationRequirements)
                .Bind(() => _managerContext.GetContractManager())
                .GetCompany(_dbContext)
                .EnsureContractBelongsToCompany(_dbContext, contractId)
                .Bind(CheckRoomAndSeasonRangeIds)
                .Bind(CheckIfAlreadyExists)
                .Map(AddAllocationRequirements)
                .Map(Build);


            async Task<Result> CheckRoomAndSeasonRangeIds(Company company) 
                => Result.Combine(await CheckIfSeasonRangesBelongToContract(allocationRequirements.Select(requirement => requirement.SeasonRangeId).ToList()),
                await CheckIfRoomsBelongToContract(company.Id, allocationRequirements.Select(requirement => requirement.RoomId).ToList()));


            Task<Result> CheckIfSeasonRangesBelongToContract(List<int> seasonRangeIds) 
                => _dbContext.CheckIfSeasonRangesBelongToContract(contractId, seasonRangeIds);


            Task<Result> CheckIfRoomsBelongToContract(int companyId, List<int> roomIds)
                => _dbContext.CheckIfRoomsBelongToContract(contractId, companyId, roomIds);
            
            
            async Task<Result> CheckIfAlreadyExists()
            {
                var seasonRangeIdsFromRequest = allocationRequirements.Select(rate => rate.SeasonRangeId).ToList();
                var roomIdsFromRequest = allocationRequirements.Select(rate => rate.RoomId).ToList();

                var existedAllocationRequirements = await _dbContext.RoomAllocationRequirements.Where(roomAllocationRequirement
                    => seasonRangeIdsFromRequest.Contains(roomAllocationRequirement.SeasonRangeId) &&
                    roomIdsFromRequest.Contains(roomAllocationRequirement.RoomId)).ToListAsync();

                return !existedAllocationRequirements.Any() ? Result.Success() : Result.Failure(CreateError());


                string CreateError()
                    => "Existed allocation requirements: " + string.Join("; ",
                        existedAllocationRequirements.Select(allocationRequirement
                            => $"{nameof(allocationRequirement.RoomId)} '{allocationRequirement.RoomId}' {nameof(allocationRequirement.SeasonRangeId)} '{allocationRequirement.SeasonRangeId}'"));
            }
            
            
            async Task<List<RoomAllocationRequirement>> AddAllocationRequirements()
            {
                var allocationRequirementEntries = Create(allocationRequirements);
                _dbContext.RoomAllocationRequirements.AddRange(allocationRequirementEntries);
                
                await _dbContext.SaveChangesAsync();
                _dbContext.DetachEntries(allocationRequirementEntries);
                
                return allocationRequirementEntries;
            }
        }


        public Task<Result<List<Models.Responses.AllocationRequirement>>> Get(int contractId, int skip, int top, List<int> roomIds = null, List<int> seasonIds = null, List<int> seasonRangeIds = null)
        {
            return _managerContext.GetContractManager()
                .GetCompany(_dbContext)
                .EnsureContractBelongsToCompany(_dbContext, contractId)
                .Map(company => GetAllocationRequirements(company.Id))
                .Map(Build);
            
            
            async Task<List<RoomAllocationRequirement>> GetAllocationRequirements(int companyId)
            {
                var seasonsAndSeasonRanges = _dbContext.Seasons.Join(_dbContext.SeasonRanges, season => season.Id, seasonRange => seasonRange.SeasonId,
                        (season, seasonRange) => new
                        {
                            Season = season,
                            SeasonRange = seasonRange
                        })
                    .Where(seasonAndSeasonRange => seasonAndSeasonRange.Season.ContractId == contractId);

                if (seasonIds != null && seasonIds.Any())
                {
                    seasonsAndSeasonRanges = seasonsAndSeasonRanges
                        .Where(seasonAndSeasonRange
                            => seasonIds.Contains(seasonAndSeasonRange.Season.Id));
                }

                if (seasonRangeIds != null && seasonRangeIds.Any())
                {
                    seasonsAndSeasonRanges = seasonsAndSeasonRanges.Where(seasonAndSeasonRange => seasonRangeIds.Contains(seasonAndSeasonRange.SeasonRange.Id));
                }

                var roomAllocationRequirement = _dbContext.RoomAllocationRequirements.Join(seasonsAndSeasonRanges,
                    allocationRequirement => allocationRequirement.SeasonRangeId,
                    seasonAndSeasonRange => seasonAndSeasonRange.SeasonRange.Id, (allocationRequirement, _) => allocationRequirement);

                if (roomIds != null && roomIds.Any())
                {
                    var contractedAccommodations = _dbContext.GetContractedAccommodations(contractId, companyId);

                    var validRoomIds = (await contractedAccommodations.Include(accommodation => accommodation.Rooms)
                        .Select(accommodation => accommodation
                            .Rooms.Where(room => roomIds.Contains(room.Id))
                            .Select(room => room.Id))
                        .SingleOrDefaultAsync()).ToList();

                    if (validRoomIds.Any())
                    {
                        roomAllocationRequirement =
                            roomAllocationRequirement.Where(allocationRequirement => validRoomIds.Contains(allocationRequirement.RoomId));
                    }
                }

                return await roomAllocationRequirement.OrderBy( allocationRequirement => allocationRequirement.Id)
                    .Skip(skip).Take(top).ToListAsync();
            }
        }


        public async Task<Result> Remove(int contractId, List<int> allocationRequirementIds)
        {
            return await _managerContext.GetContractManager()
                .GetCompany(_dbContext)
                .EnsureContractBelongsToCompany(_dbContext, contractId)
                .Map(company => GetAllocationRequirements())
                .Tap(Remove);


            async Task<List<RoomAllocationRequirement>> GetAllocationRequirements()
            {
                var seasonsRangeIds = (await _dbContext.GetSeasons()
                    .Where(season => season.ContractId == contractId)
                    .Select(season => season.SeasonRanges).ToListAsync()).SelectMany(seasonRanges=>seasonRanges)
                    .Select(seasonRange => seasonRange.Id);
                    

                return await _dbContext.RoomAllocationRequirements
                    .Where(allocationRequirement => seasonsRangeIds.Contains(allocationRequirement.SeasonRangeId) && 
                        allocationRequirementIds.Contains(allocationRequirement.Id))
                    .ToListAsync();
            }


            async Task Remove(List<RoomAllocationRequirement> allocationRequirements)
            {
                _dbContext.RoomAllocationRequirements.RemoveRange(allocationRequirements);
                
                await _dbContext.SaveChangesAsync();
            }
        }
        

        private List<Models.Responses.AllocationRequirement> Build(List<RoomAllocationRequirement> allocationRequirement)
            => allocationRequirement.Select(Build).ToList();


        private Models.Responses.AllocationRequirement Build(RoomAllocationRequirement allocationRequirement)
            => new Models.Responses.AllocationRequirement(allocationRequirement.Id, allocationRequirement.SeasonRangeId, allocationRequirement.RoomId,
                allocationRequirement.ReleaseDays, allocationRequirement.Allotment, allocationRequirement.MinimumLengthOfStay);

        
        private List<RoomAllocationRequirement> Create(List<Models.Requests.AllocationRequirement> requirements)
            => requirements.Select(Create).ToList();
        

        private RoomAllocationRequirement Create(Models.Requests.AllocationRequirement requirement)
            => new RoomAllocationRequirement
            {
                RoomId = requirement.RoomId,
                SeasonRangeId = requirement.SeasonRangeId,
                ReleaseDays = requirement.ReleaseDays,
                Allotment = requirement.Allotment,
                MinimumLengthOfStay = requirement.MinimumLengthOfStay
            };


        private Result Validate(List<Models.Requests.AllocationRequirement> allocationRequirements)
            => ValidationHelper.Validate(allocationRequirements, new AllocationRequirementsValidator());

        
        private readonly DirectContractsDbContext _dbContext;
        private readonly IContractManagerContextService _managerContext;
    }
}