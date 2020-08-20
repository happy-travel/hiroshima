using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.Data.Extensions;
using HappyTravel.Hiroshima.DirectContracts.Services.Management;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public class SeasonManagementService : ISeasonManagementService
    {
        public SeasonManagementService(DirectContractsDbContext dbContext, IContractManagerContextService contractManagerContext,
            IContractManagementRepository contractManagementRepository)
        {
            _dbContext = dbContext;
            _contractManagerContext = contractManagerContext;
            _contractManagementRepository = contractManagementRepository;
        }


        public Task<Result<List<Models.Responses.Season>>> Add(int contractId, List<string> names)
        {
            return _contractManagerContext.GetContractManager()
                .Ensure(contractManager => _dbContext.DoesContractBelongToContractManager(contractId, contractManager.Id),
                    $"Contract '{contractId}' doesn't belong to the contract manager")
                .Map(contractManager => AddSeasonNames())
                .Map(Build);


             async Task<List<Season>> AddSeasonNames()
             {
                 var seasons = names.Select(name => new Season
                     {
                         ContractId = contractId,
                         Name = name
                     })
                     .ToList();
                 _dbContext.Seasons.AddRange(seasons);
                 await _dbContext.SaveChangesAsync();
                 
                 _dbContext.DetachEntries(seasons);

                 return seasons;
             }
        }


        public Task<Result<List<Models.Responses.Season>>> Get(int contractId)
        {
            return _contractManagerContext.GetContractManager()
                .Ensure(contractManager => CheckIfContractBelongToContractManager(contractId, contractManager.Id),
                    $"Contract '{contractId}' doesn't belong to the contract manager")
                .Map(contractManager => GetSeasons())
                .Map(Build);

            
            Task<List<Season>> GetSeasons()
                => _dbContext.Seasons.Where(season => season.ContractId == contractId).ToListAsync();
        }


        public Task<Result> Remove(int contractId, int seasonId)
        {
            return _contractManagerContext.GetContractManager()
                .Ensure(contractManager => CheckIfContractBelongToContractManager(contractId, contractManager.Id),
                    $"Contract '{contractId}' doesn't belong to the contract manager")
                .Bind(contractManager => GetSeason())
                .Ensure(CheckIfSeasonDoesntHaveAnySeasonRanges, 
                    $"Season with {nameof(seasonId)} '{seasonId}' have an associated date range" )
                .Map(Remove)
                .Finally(result => result.IsSuccess ? Result.Success() : Result.Failure(result.Error));


            async Task<Result<Season>> GetSeason()
            {
                var season = await _dbContext.Seasons.SingleOrDefaultAsync(season => season.Id == seasonId && season.ContractId == contractId);

                return season != null
                    ? Result.Success(season)
                    : Result.Failure<Season>($"Failed to get the {nameof(season)} '{seasonId}'");
            }
            
            
           async Task<bool> CheckIfSeasonDoesntHaveAnySeasonRanges(Season season)
           {
                var seasonRanges = await _dbContext.SeasonRanges
                    .Where(seasonRange => seasonRange.SeasonId == season.Id)
                    .ToListAsync();

                return !seasonRanges.Any();
           }
            
            
            async Task Remove(Season season)
            {
                _dbContext.Seasons.Remove(season);
                
                await _dbContext.SaveChangesAsync();
            }
        }


        public Task<Result<List<Models.Responses.SeasonRange>>> SetSeasonRanges(int contractId, List<Models.Requests.SeasonRange> seasonRanges)
        {
            return _contractManagerContext.GetContractManager()
                .Bind(contractManager => Validate(contractManager.Id, contractId, seasonRanges))
                .Map(ReplaceSeasonRanges)
                .Map(Build);

            
            async Task RemovePreviousSeasonRanges()
            {
                var seasonRangesToDelete = await _dbContext.GetSeasonsAndSeasonRanges()
                    .Where(seasonsAndSeasonRanges => seasonsAndSeasonRanges.Season.ContractId == contractId)
                    .Select(seasonsAndSeasonRanges => seasonsAndSeasonRanges.SeasonRange)
                    .ToListAsync();

                if (!seasonRangesToDelete.Any())
                    return;
                
                _dbContext.SeasonRanges.RemoveRange(seasonRangesToDelete);

                await _dbContext.SaveChangesAsync();
            }
            

            async Task<List<SeasonRange>> ReplaceSeasonRanges()
            {
                await RemovePreviousSeasonRanges();
                return await AddSeasonRanges();
            }
            
            
            async Task<List<SeasonRange>> AddSeasonRanges()
            {
                var newSeasonRanges = Create(seasonRanges);
                _dbContext.SeasonRanges.AddRange(newSeasonRanges);
                await _dbContext.SaveChangesAsync();
                
                _dbContext.DetachEntries(newSeasonRanges);
                
                return newSeasonRanges;
            }


            List<SeasonRange> Create(List<Models.Requests.SeasonRange> seasonRanges) => seasonRanges.Select(CreateSeasonRange).ToList();
            
            
            SeasonRange CreateSeasonRange(Models.Requests.SeasonRange seasonRange) 
                => new SeasonRange
            {
                SeasonId = seasonRange.SeasonId,
                StartDate = seasonRange.StartDate,
                EndDate = seasonRange.EndDate
            };
        }


        public Task<Result<List<Models.Responses.SeasonRange>>> GetSeasonRanges(int contractId)
        {
            return _contractManagerContext.GetContractManager()
                .Ensure(contractManager => CheckIfContractBelongToContractManager(contractId, contractManager.Id),
                    $"Contract '{contractId}' doesn't belong to the contract manager")
                .Map(contractManager => GetSeasonRanges())
                .Map(Build);

            
            async Task<List<SeasonRange>> GetSeasonRanges()
                => await _dbContext.GetSeasonsAndSeasonRanges ()
                    .Where(seasonAndSeasonRange => seasonAndSeasonRange.Season.ContractId == contractId)
                    .Select(seasonAndSeasonRange => seasonAndSeasonRange.SeasonRange)
                    .ToListAsync();
        }
        
        
        private async Task<Result> Validate(int contractManagerId, int contractId, List<Models.Requests.SeasonRange> seasonRanges)
        {
            var contract = await _contractManagementRepository.GetContract(contractId, contractManagerId);
            if (contract == null)
                return Result.Failure($"Contract '{contractId}' doesn't belong to the contract manager");

            var dateRanges = GetSortedDateRanges();

            return Result.Combine(CheckIfRangesInAscendingOrder(), CheckIfRangesCoverContractedPeriod(), await CheckIfSeasonsBelongToContract());


            Result CheckIfRangesInAscendingOrder()
            {
                var previousDateRange = dateRanges.First();
                foreach (var dateRange in dateRanges.Skip(1))
                {
                    var daysBetweenSeasons = (dateRange.startDate.Date - previousDateRange.endDate.Date).Days;
                    if (Math.Abs(daysBetweenSeasons) != 1)
                        return Result.Failure(
                            $"Incorrect interval between date ranges: '{previousDateRange.startDate} - {previousDateRange.endDate}' and '{dateRange.startDate} - {dateRange.endDate}'");

                    previousDateRange = dateRange;
                }

                return Result.Success();
            }


            Result CheckIfRangesCoverContractedPeriod()
            {
                var contractStartDate = contract.ValidFrom;
                var contractEndDate = contract.ValidTo;
                var firstSeason = dateRanges.First();
                var lastSeason = dateRanges.Last();

                return contractStartDate.Date == firstSeason.startDate.Date && contractEndDate.Date == lastSeason.endDate
                    ? Result.Success()
                    : Result.Failure($"Contracted period '{contract.ValidFrom} - {contract.ValidTo}' must be fully covered");
            }


            async Task<Result> CheckIfSeasonsBelongToContract()
            {
               var requestSeasonIds = seasonRanges.Select(seasonRange => seasonRange.SeasonId).ToList();
               var validRequestSeasonIds = await _dbContext.Seasons
                   .Where(season => requestSeasonIds.Contains(season.Id) && season.ContractId == contractId)
                   .Select(season => season.Id)
                   .ToListAsync();

               var invalidRequestIds = requestSeasonIds.Except(validRequestSeasonIds).ToList();
               if (invalidRequestIds.Any())
                   return Result.Failure($"Incorrect season ids '{string.Join(", ", invalidRequestIds)}'");

               return Result.Success();
            }
            

            List<(DateTime startDate, DateTime endDate)> GetSortedDateRanges()
                => seasonRanges.Select(seasonRange => (seasonRange.StartDate, seasonRange.EndDate)).OrderBy(dateRange => dateRange.StartDate).ToList();
        }
        
        
        private List<Models.Responses.SeasonRange> Build(List<SeasonRange> seasonRanges)
            => seasonRanges.Select(BuildSeasonRange).ToList();


        private Models.Responses.SeasonRange BuildSeasonRange(SeasonRange seasonRange) 
            => new Models.Responses.SeasonRange(seasonRange.Id, seasonRange.SeasonId, seasonRange.StartDate, seasonRange.EndDate);
        

        private List<Models.Responses.Season> Build(List<Season> seasons) 
            => seasons.Select(BuildSeason).ToList();


        private Models.Responses.Season BuildSeason(Season season) 
            => new Models.Responses.Season(season.Id, season.Name);

        
        private async Task<bool> CheckIfContractBelongToContractManager(int contractId, int contractManagerId)
            => await _contractManagementRepository.GetContract(contractId, contractManagerId) != null;
        
        
        private readonly IContractManagementRepository _contractManagementRepository;
        private readonly IContractManagerContextService _contractManagerContext;
        private readonly DirectContractsDbContext _dbContext;
    }
}