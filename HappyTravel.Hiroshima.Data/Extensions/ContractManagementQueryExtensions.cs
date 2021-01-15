using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Common.Models.Accommodations;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms;
using HappyTravel.Hiroshima.Common.Models.Seasons;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace HappyTravel.Hiroshima.Data.Extensions
{
    public static class ContractManagementQueryExtensions
    {
        public static async Task<Result> CheckIfRoomsBelongToContract(this DirectContractsDbContext dbContext, int contractId, int serviceSupplierId, List<int> roomIds)
        {
            if (!roomIds.Any())
                return Result.Success();
            
            var availableRoomIds = await dbContext.Rooms
                .Join(dbContext.Accommodations, room => room.AccommodationId, accommodation => accommodation.Id, (room, accommodation) => new
                {
                    room,
                    accommodation
                })
                .Join(dbContext.ContractAccommodationRelations, roomAndAccommodation=> roomAndAccommodation.accommodation.Id, relation => relation.AccommodationId, (roomAndAccommodation, relation) 
                    => new {roomAndAccommodation.accommodation, roomAndAccommodation.room, relation})
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
                    accommodationAndRoomAndRelationAndContract.contract.ServiceSupplierId == serviceSupplierId)
                .Select(accommodationAndRoomAndRelationAndContract => accommodationAndRoomAndRelationAndContract.room.Id)
                .ToListAsync();
                
            var notBelongIds = roomIds.Except(availableRoomIds).ToList();

            return notBelongIds.Any() 
                ? Result.Failure($"Invalid room ids: {string.Join(", ", notBelongIds)}") 
                : Result.Success();
        }
        
        
        public static async Task<Result> CheckIfSeasonsBelongToContract(this DirectContractsDbContext dbContext, int contractId, List<int> seasonIds)
        {
            var availableSeasonIds =
                await dbContext.Seasons.Where(s => seasonIds.Contains(s.Id) && s.ContractId == contractId).Select(s => s.Id).ToListAsync();

            var notBelongIds = seasonIds.Except(availableSeasonIds).ToList();

            return notBelongIds.Any() 
                ? Result.Failure($"Season ids {string.Join(", ", notBelongIds)} do not belong to the contract manager") 
                : Result.Success();
        }
        
        
        public static async Task<Result> CheckIfSeasonRangesBelongToContract(this DirectContractsDbContext dbContext, int contractId, List<int> seasonRangeIds)
        {
            var allContractRangeIds = (await dbContext.GetSeasons()
                .Where(season => season.ContractId == contractId)
                .Select(season => season.SeasonRanges)
                .ToListAsync())
                .SelectMany(season => season)
                .Select(seasonRange => seasonRange.Id).ToList();

            var notBelongIds = seasonRangeIds.Except(allContractRangeIds).ToList();
            
            return notBelongIds.Any() 
                ? Result.Failure($"Season range ids {string.Join(", ", notBelongIds)} do not belong to the contract manager") 
                : Result.Success();
        }
        

        public static IQueryable<Accommodation> GetContractedAccommodations(this DirectContractsDbContext dbContext, int contractId, int serviceSupplierId) 
            => dbContext.Accommodations
            .Join(dbContext.ContractAccommodationRelations, accommodation => accommodation.Id, relation => relation.AccommodationId,
                (accommodation, relation) => new {accommodation, relation})
            .Join(dbContext.Contracts, accommodationAndRelation => accommodationAndRelation.relation.ContractId, contract => contract.Id,
                (accommodationAndRelation, contract) => new {accommodationAndRelation, contract})
            .Where(accommodationAndRelationAndContract => accommodationAndRelationAndContract.contract.ServiceSupplierId == serviceSupplierId &&
                accommodationAndRelationAndContract.contract.Id == contractId)
            .Select(accommodationAndRelationAndContract => accommodationAndRelationAndContract.accommodationAndRelation.accommodation);


        public static async Task<bool> DoesAccommodationBelongToServiceSupplier(this DirectContractsDbContext dbContext, int accommodationId, int serviceSupplierId)
            => await dbContext.Accommodations.AnyAsync(a => a.ServiceSupplierId == serviceSupplierId && a.Id == accommodationId);


        public static async Task<bool> DoesContractBelongToServiceSupplier(this DirectContractsDbContext dbContext, int contractId, int serviceSupplierId)
            => await dbContext.Contracts.AnyAsync(c => c.ServiceSupplierId == serviceSupplierId && c.Id == contractId);


        public static IIncludableQueryable<Season, List<SeasonRange>> GetSeasons(this DirectContractsDbContext dbContext)
            => dbContext.Seasons.Include(season => season.SeasonRanges);


        public static IIncludableQueryable<Accommodation, List<Room>> GetAccommodations(this DirectContractsDbContext dbContext)
            => dbContext.Accommodations.Include(accommodation => accommodation.Rooms);
    }
}