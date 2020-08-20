using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.Data.Extensions;
using HappyTravel.Hiroshima.Data.Models;
using HappyTravel.Hiroshima.Data.Models.Rooms;
using HappyTravel.Hiroshima.DirectManager.RequestValidators;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public class AllocationRequirementManagementService : IAllocationRequirementManagementService
    {
        public AllocationRequirementManagementService(DirectContractsDbContext dbContext, IContractManagerContextService contractManagerContextService)
        {
            _contractManagerContextService = contractManagerContextService;
            _dbContext = dbContext;
        }


        public Task<Result<List<Models.Responses.AllocationRequirement>>> Add(int contractId, List<Models.Requests.AllocationRequirement> allocationRequirements)
        {
            return Validate(allocationRequirements)
                .Bind(() => _contractManagerContextService.GetContractManager())
                .Ensure(contractManager => _dbContext.DoesContractBelongToContractManager(contractId, contractManager.Id),
                    $"Contract '{contractId}' doesn't belong to the contract manager")
                .Bind(CheckRoomAndSeasonRangeIds)
                .Map(AddAllocationRequirements)
                .Map(Build);

            
            async Task<List<RoomAllocationRequirement>> AddAllocationRequirements()
            {
                var allocationRequirementEntries = Create(allocationRequirements);
                _dbContext.RoomAllocationRequirements.AddRange(allocationRequirementEntries);
                _dbContext.DetachEntries(allocationRequirementEntries);
                await _dbContext.SaveChangesAsync();

                return allocationRequirementEntries;
            }


            async Task<Result> CheckRoomAndSeasonRangeIds(ContractManager contractManager)
            {
                return Result.Combine(await CheckIfSeasonRangesBelongToContract(allocationRequirements.Select(requirement => requirement.SeasonRangeId).ToList()),
                    await CheckIfRoomsBelongToContract(contractManager, allocationRequirements.Select(requirement => requirement.RoomId).ToList()));
            }     
            
            
            Task<Result> CheckIfSeasonRangesBelongToContract(List<int> seasonRangeIds) 
                => _dbContext.CheckIfSeasonRangesBelongToContract(contractId, seasonRangeIds);


            Task<Result> CheckIfRoomsBelongToContract(ContractManager contractManager, List<int> roomIds)
                => _dbContext.CheckIfRoomsBelongToContract(contractId, contractManager.Id, roomIds);
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
        private readonly IContractManagerContextService _contractManagerContextService;
    }
}