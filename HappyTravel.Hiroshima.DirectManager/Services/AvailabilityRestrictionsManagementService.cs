using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms;
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.Data.Extensions;
using HappyTravel.Hiroshima.DirectManager.Infrastructure;
using HappyTravel.Hiroshima.DirectManager.Infrastructure.Extensions;
using HappyTravel.Hiroshima.DirectManager.RequestValidators;
using Microsoft.EntityFrameworkCore;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public class AvailabilityRestrictionsManagementService : IAvailabilityRestrictionsManagementService
    {
        public AvailabilityRestrictionsManagementService(DirectContractsDbContext dbContext, IContractManagerContextService managerContextService)
        {
            _dbContext = dbContext;
            _managerContext = managerContextService;
        }


        public Task<Result<List<Models.Responses.AvailabilityRestriction>>> Set(int contractId,
            List<Models.Requests.AvailabilityRestriction> availabilityRestrictions)
        {
            return _managerContext.GetContractManager()
                .GetCompany(_dbContext)
                .Tap(company => Validate(company.Id, contractId, availabilityRestrictions))
                .Map(company => Replace())
                .Map(Build);

            
            
            async Task<List<RoomAvailabilityRestriction>> Replace()
            {
                availabilityRestrictions = availabilityRestrictions
                    .Where(availabilityRestriction => availabilityRestriction.Restriction != AvailabilityRestrictions.FreeSale).ToList();
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


        public Task<Result<List<Models.Responses.AvailabilityRestriction>>> Get(int contractId, int skip, int top, List<int> roomIds, DateTime? fromDate, DateTime? toDate, AvailabilityRestrictions? restriction)
        {
            return _managerContext.GetContractManager()
                .GetCompany(_dbContext)
                .Map(company => Get(company.Id))
                .Map(Build);


            async Task<List<RoomAvailabilityRestriction>> Get(int companyId)
            {
                var availabilityRestrictionsQueryable = _dbContext.RoomAvailabilityRestrictions.Where(availabilityRestriction => availabilityRestriction.ContractId == contractId);
                
                if (roomIds != null && roomIds.Any())
                {
                    var validRoomIdsQueryable = _dbContext.GetContractedAccommodations(contractId, companyId)
                        .Join(_dbContext.Rooms.Where(room => roomIds.Contains(room.Id)), accommodation => accommodation.Id, room => room.AccommodationId,
                            (accommodation, room) => room.Id)
                        .OrderBy(id => id);
                        
                    availabilityRestrictionsQueryable = availabilityRestrictionsQueryable.Where(availabilityRestriction => validRoomIdsQueryable.Contains(availabilityRestriction.RoomId));
                }

                if (fromDate != null)
                {
                    availabilityRestrictionsQueryable = availabilityRestrictionsQueryable.Where(availabilityRestriction => fromDate.Value.Date <= availabilityRestriction.FromDate);
                }

                if (toDate != null)
                {
                    availabilityRestrictionsQueryable = availabilityRestrictionsQueryable.Where(availabilityRestriction => availabilityRestriction.FromDate <= toDate.Value.Date);
                }

                if (restriction != null)
                {
                    availabilityRestrictionsQueryable = availabilityRestrictionsQueryable.Where(availabilityRestriction => availabilityRestriction.Restriction == restriction.Value);
                }

                return await availabilityRestrictionsQueryable
                    .OrderBy(availabilityRestriction => availabilityRestriction.Id)
                    .Skip(skip)
                    .Take(top)
                    .ToListAsync();
            }
        }

        
        public async Task<Result> Remove(int contractId, List<int> availabilityRestrictionIds)
        {
          return await _managerContext.GetContractManager()
                .GetCompany(_dbContext)
                .EnsureContractBelongsToCompany(_dbContext, contractId)
                .Bind(company => GetAvailabilityRestrictionsToRemove(company.Id))
                .Map(RemoveAvailabilityRestrictions);
                
          
            async Task<Result<List<RoomAvailabilityRestriction>>> GetAvailabilityRestrictionsToRemove(int companyId)
            {
                var availabilityRestrictions = await _dbContext.RoomAvailabilityRestrictions
                    .Where(availabilityRestriction => availabilityRestrictionIds.Contains(availabilityRestriction.Id))
                    .ToListAsync();

                if (availabilityRestrictions == null || !availabilityRestrictions.Any())
                    return Result.Success(availabilityRestrictions);

                var checkingResult = await _dbContext.CheckIfRoomsBelongToContract(contractId, companyId,
                    availabilityRestrictions.Select(availabilityRestriction => availabilityRestriction.RoomId).ToList());

                return checkingResult.IsFailure 
                    ? Result.Failure<List<RoomAvailabilityRestriction>>(checkingResult.Error) 
                    : Result.Success(availabilityRestrictions);
            }
                
            async Task RemoveAvailabilityRestrictions(List<RoomAvailabilityRestriction> availabilityRestrictions)
            {
                if (!availabilityRestrictions.Any())
                    return;

                _dbContext.RoomAvailabilityRestrictions.RemoveRange(availabilityRestrictions);
                await _dbContext.SaveChangesAsync();
            }
        }
        
    
        private async Task<Result> Validate(int companyId, int contractId, List<Models.Requests.AvailabilityRestriction> availabilityRestrictions)
        {
            var contract = await _dbContext.Contracts.SingleOrDefaultAsync(c => c.Id == contractId && c.CompanyId == companyId);
            if (contract == null)
                return Result.Failure($"Contract '{contractId}' doesn't belong to the company");
            
            return Result.Combine(GenericValidator<List<Models.Requests.AvailabilityRestriction>>.Validate(
                configure => configure.RuleFor(restrictions => restrictions).SetValidator(new AvailabilityRestrictionsValidator(contract)), availabilityRestrictions),
                await _dbContext.CheckIfRoomsBelongToContract(contractId, companyId,
                    availabilityRestrictions.Select(availabilityRestriction => availabilityRestriction.RoomId).ToList()));
        }


        private RoomAvailabilityRestriction Create(int contractId, Models.Requests.AvailabilityRestriction availabilityRestriction)
            => new RoomAvailabilityRestriction
            {
                ContractId = contractId,
                RoomId = availabilityRestriction.RoomId,
                FromDate = availabilityRestriction.FromDate.Date,
                ToDate = availabilityRestriction.ToDate.Date,
                Restriction = availabilityRestriction.Restriction
            };

        
        private List<RoomAvailabilityRestriction> Create(int contractId, List<Models.Requests.AvailabilityRestriction> availabilityRestrictions)
            => availabilityRestrictions.Select(availabilityRestriction => Create(contractId, availabilityRestriction)).ToList();

        
        private Models.Responses.AvailabilityRestriction Build(RoomAvailabilityRestriction availabilityRestriction)
            => new Models.Responses.AvailabilityRestriction(availabilityRestriction.Id, availabilityRestriction.FromDate, availabilityRestriction.ToDate, availabilityRestriction.RoomId, availabilityRestriction.Restriction);


        private List<Models.Responses.AvailabilityRestriction> Build(List<RoomAvailabilityRestriction> availabilityRestrictions)
            => availabilityRestrictions.Select(Build).ToList();
        
        
        private readonly IContractManagerContextService _managerContext;
        private readonly DirectContractsDbContext _dbContext;
    }
}