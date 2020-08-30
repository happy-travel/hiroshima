using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms;
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.Data.Extensions;
using HappyTravel.Hiroshima.Data.Models.Rooms;
using HappyTravel.Hiroshima.DirectManager.Infrastructure;
using HappyTravel.Hiroshima.DirectManager.Infrastructure.Extensions;
using HappyTravel.Hiroshima.DirectManager.RequestValidators;
using Microsoft.EntityFrameworkCore;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public class AvailabilityRestrictionsManagementService : IAvailabilityRestrictionsManagementService
    {
        public AvailabilityRestrictionsManagementService(DirectContractsDbContext dbContext, IContractManagerContextService contractManagerContext)
        {
            _dbContext = dbContext;
            _contractManagerContext = contractManagerContext;
        }


        public Task<Result<List<Models.Responses.AvailabilityRestriction>>> Set(int contractId,
            List<Models.Requests.AvailabilityRestriction> availabilityRestrictions)
        {
            return _contractManagerContext.GetContractManager()
                .Tap(contractManager => Validate(contractManager.Id, contractId, availabilityRestrictions))
                .Map(contractManager => Replace())
                .Map(Build);

            
            async Task<List<RoomAvailabilityRestriction>> Replace()
            {
                var groupedRestrictions = availabilityRestrictions.GroupBy(availabilityRestriction => availabilityRestriction.RoomId)
                    .ToDictionary(group => group.Key, group => group.ToList());
                
                var newAvailabilityRestrictions = new List<RoomAvailabilityRestriction>(availabilityRestrictions.Count);
                foreach (var availabilityRestrictionGroup in groupedRestrictions)
                {
                    var dbAvailabilityRestrictions = Create(contractId, availabilityRestrictionGroup.Value);
                    newAvailabilityRestrictions.AddRange(await ReplaceAvailabilityRestrictions(availabilityRestrictionGroup.Key, dbAvailabilityRestrictions));
                }

                return newAvailabilityRestrictions;
            }


            async Task<List<RoomAvailabilityRestriction>> ReplaceAvailabilityRestrictions(int roomId,
                List<RoomAvailabilityRestriction> newAvailabilityRestrictions)
            {
                var previousAvailabilityRestrictions = await _dbContext.RoomAvailabilityRestrictions.Where(availabilityRestriction
                    => availabilityRestriction.ContractId == contractId && availabilityRestriction.RoomId == roomId).ToListAsync();
                _dbContext.RoomAvailabilityRestrictions.RemoveRange(previousAvailabilityRestrictions);
                _dbContext.RoomAvailabilityRestrictions.AddRange(newAvailabilityRestrictions);
                await _dbContext.SaveChangesAsync();

                _dbContext.DetachEntries(newAvailabilityRestrictions);

                return newAvailabilityRestrictions;
            }
        }


        public Task<Result<Models.Responses.AvailabilityRestriction>> Get(int contractId, List<int> roomIds, DateTime? fromDate, DateTime? toDate, AvailabilityRestrictions? restriction) => throw new NotImplementedException();

        
        private async Task<Result> Validate(int contractManagerId, int contractId, List<Models.Requests.AvailabilityRestriction> availabilityRestrictions)
        {
            var contract = await _dbContext.Contracts.SingleOrDefaultAsync(contract => contract.Id == contractId && contract.ContractManagerId == contractManagerId);
            if (contract == null)
                return Result.Failure($"Contract '{contractId}' doesn't belong to the contract manager");
            
            return Result.Combine(GenericValidator<List<Models.Requests.AvailabilityRestriction>>.Validate(
                configure => configure.RuleFor(restrictions => restrictions).SetValidator(new AvailabilityRestrictionsValidator(contract)), availabilityRestrictions),
                await _dbContext.CheckIfRoomsBelongToContract(contractId, contractManagerId,
                    availabilityRestrictions.Select(availabilityRestriction => availabilityRestriction.RoomId).ToList()));
        }


        private RoomAvailabilityRestriction Create(int contractId, Models.Requests.AvailabilityRestriction availabilityRestriction)
            => new RoomAvailabilityRestriction
            {
                ContractId = contractId,
                RoomId = availabilityRestriction.RoomId,
                FromDate = availabilityRestriction.FromDate,
                ToDate = availabilityRestriction.ToDate,
                Restriction = availabilityRestriction.Restriction
            };

        
        private List<RoomAvailabilityRestriction> Create(int contractId, List<Models.Requests.AvailabilityRestriction> availabilityRestrictions)
            => availabilityRestrictions.Select(availabilityRestriction => Create(contractId, availabilityRestriction)).ToList();

        
        private Models.Responses.AvailabilityRestriction Build(RoomAvailabilityRestriction availabilityRestriction)
            => new Models.Responses.AvailabilityRestriction(availabilityRestriction.Id, availabilityRestriction.FromDate, availabilityRestriction.ToDate, availabilityRestriction.RoomId, availabilityRestriction.Restriction);


        private List<Models.Responses.AvailabilityRestriction> Build(List<RoomAvailabilityRestriction> availabilityRestrictions)
            => availabilityRestrictions.Select(Build).ToList();
        
        
        private readonly IContractManagerContextService _contractManagerContext;
        private readonly DirectContractsDbContext _dbContext;
    }
}