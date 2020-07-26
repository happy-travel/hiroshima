using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.Data.Extensions;
using HappyTravel.Hiroshima.Data.Models;
using HappyTravel.Hiroshima.DirectContracts.Services.Management;
using HappyTravel.Hiroshima.DirectManager.RequestValidators;
using Microsoft.EntityFrameworkCore;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public class SeasonManagementService : ISeasonManagementService
    {
        public SeasonManagementService(DirectContractsDbContext dbContext, IContractManagerContextService contractManagerContext, IContractManagementRepository contractManagementRepository)
        {
            _dbContext = dbContext;
            _contractManagerContext = contractManagerContext;
            _contractManagementRepository = contractManagementRepository;
        }


        public Task<Result<List<Models.Responses.Season>>> Get(int contractId)
        {
            return _contractManagerContext.GetContractManager()
                .Ensure(contractManager => DoesContractBelongToContractManager(contractId, contractManager.Id),
                    $"Failed to get a contract by {nameof(contractId)} '{contractId}'")
                .Bind(async contractManager 
                    =>
                {
                    var seasons = await _dbContext.Seasons
                        .Where(s => s.ContractId == contractId).ToListAsync();
                    
                    return Result.Success(CreateResponse(seasons));
                });
        }


        public Task<Result<List<Models.Responses.Season>>> Replace(int contractId, List<Models.Requests.Season> seasons)
        {
            return Validate(seasons)
                .Bind(() => _contractManagerContext.GetContractManager())
                .Ensure(contractManagerContext => DoesContractBelongToContractManager(contractId, contractManagerContext.Id),
                    $"Failed to get a contract by {nameof(contractId)} '{contractId}'")
                .Tap(async contractManager =>
                {
                    var allContractSeasons = await _dbContext.Seasons.Where(s => s.ContractId == contractId)
                        .ToListAsync();
                    _dbContext.Seasons.RemoveRange(allContractSeasons);
                })
                .Bind(contractManager =>
                {
                    var newSeasons = CreateSeasons(contractId, seasons);
                    _dbContext.Seasons.AddRange();
                    _dbContext.SaveChangesAsync();
                    _dbContext.DetachEntries(newSeasons);
                    return Result.Success(CreateResponse(newSeasons));
                });
        }


        public Task<Result> Remove(int contractId, List<int> seasonIds)
        {
            return _contractManagerContext.GetContractManager()
                .Ensure(contractManager => DoesContractBelongToContractManager(contractId, contractManager.Id),
                    $"Failed to get a contract by {nameof(contractId)} '{contractId}'")
                .Bind(async contractManager =>
                {
                    var seasons = await _dbContext.Seasons
                        .Where(s => s.ContractId == contractId && seasonIds.Contains(s.ContractId))
                        .ToListAsync();
                    _dbContext.Seasons.RemoveRange(seasons);
                    await _dbContext.SaveChangesAsync();
                    return Result.Success();
                });
        }


        public Result Validate(List<Models.Requests.Season> seasons)
        {
            return Result.Combine(ValidationHelper.Validate(seasons, new SeasonValidator()), ValidateSeasonIntervals());
            
            
            Result ValidateSeasonIntervals()
            {
                var previousSeason = seasons[0];
                for (var i = 1; i < seasons.Count; i++)
                {
                    var season = seasons[i];
                    var daysBetweenSeasons = (season.StartDate.Date - previousSeason.EndDate.Date).Days;
                    if (Math.Abs(daysBetweenSeasons) != 1)
                        return Result.Failure($"An unacceptable interval between seasons: '{previousSeason.Name}' and '{season.Name}'");
                    
                    previousSeason = season;
                }
                
                return Result.Success();
            }
        }


        private List<Season> CreateSeasons(int contractId, List<Models.Requests.Season> seasons) 
            => seasons.Select(s => new Season
        {
            Name = s.Name,
            ContractId = contractId,
            StartDate = s.StartDate,
            EndDate = s.EndDate
        }).ToList();


        private List<Models.Responses.Season> CreateResponse(List<Season> seasons) 
            => seasons.Select(
                s => new Models.Responses.Season(s.Id, s.Name, s.StartDate, s.EndDate, s.ContractId))
            .ToList();


        private async Task<bool> DoesContractBelongToContractManager(int contractId, int contractManagerId)
            => await _contractManagementRepository.GetContract(contractManagerId, contractId) != null;

        
        private readonly IContractManagementRepository _contractManagementRepository;
        private readonly IContractManagerContextService _contractManagerContext;
        private readonly DirectContractsDbContext _dbContext;
    }
}