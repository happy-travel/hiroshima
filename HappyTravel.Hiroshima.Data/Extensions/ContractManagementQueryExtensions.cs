using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Data.Models.Accommodations;
using Microsoft.EntityFrameworkCore;

namespace HappyTravel.Hiroshima.Data.Extensions
{
    public static class ContractManagementQueryExtensions
    {
        public static async Task<Result> CheckIfRoomsBelongToContract(this DirectContractsDbContext dbContext, int contractId, int contractManagerId, List<int> roomIds)
        {
            if (!roomIds.Any())
                return Result.Success();
            
            var availableRoomIds = await dbContext.Rooms
                .Join(dbContext.Accommodations, room => room.AccommodationId, accommodation => accommodation.Id, (room, accommodation) => new
                {
                    room,
                    accommodation
                })
                .Join(dbContext.ContractAccommodationRelations, roomAndAccommodation=> roomAndAccommodation.accommodation.Id, relation => relation.AccommodationId, (roomAndAccommodation, relation) => new {roomAndAccommodation.accommodation, roomAndAccommodation.room, relation})
                .Join(dbContext.Contracts, accommodationAndRoomAndRelation => accommodationAndRoomAndRelation.relation.ContractId , contract => contract.Id,
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
        
        
        public static async Task<Result> CheckIfSeasonsBelongToContract(this DirectContractsDbContext dbContext, int contractId, List<int> seasonIds)
        {
            var availableSeasonIds =
                await dbContext.Seasons.Where(s => seasonIds.Contains(s.Id) && s.ContractId == contractId).Select(s => s.Id).ToListAsync();

            var inappropriateSeasonIds = seasonIds.Except(availableSeasonIds).ToList();

            return inappropriateSeasonIds.Any() ? Result.Failure($"Inappropriate season ids: {string.Join(", ", inappropriateSeasonIds)}") : Result.Success();
        }


        public static IQueryable<Accommodation> GetContractedAccommodations(this DirectContractsDbContext dbContext, int contractId, int contractManagerId) 
            => dbContext.Accommodations
            .Join(dbContext.ContractAccommodationRelations, accommodation => accommodation.Id, relation => relation.AccommodationId,
                (accommodation, relation) => new {accommodation, relation})
            .Join(dbContext.Contracts, accommodationAndRelation => accommodationAndRelation.relation.ContractId, contract => contract.Id,
                (accommodationAndRelation, contract) => new {accommodationAndRelation, contract})
            .Where(accommodationAndRelationAndContract => accommodationAndRelationAndContract.contract.ContractManagerId == contractManagerId &&
                accommodationAndRelationAndContract.contract.Id == contractId)
            .Select(accommodationAndRelationAndContract => accommodationAndRelationAndContract.accommodationAndRelation.accommodation);


        public static async Task<bool> DoesContractBelongToContractManager(this DirectContractsDbContext dbContext, int contractId, int contractManagerId)
            => await dbContext.Contracts.SingleOrDefaultAsync(c => c.ContractManagerId == contractManagerId && c.Id == contractId) != null;
        
        
        public static IQueryable<SeasonAndSeasonRange> GetSeasonsAndSeasonRanges(this DirectContractsDbContext dbContext)
            => dbContext.Seasons.Join(dbContext.SeasonRanges, season => season.Id, seasonRange => seasonRange.SeasonId,
                (season, seasonRange) => new SeasonAndSeasonRange
                {
                    Season = season,
                    SeasonRange = seasonRange
                });
    }
}