using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace HappyTravel.Hiroshima.DirectManager.Infrastructure.Extensions
{
    public static class ContractManagerExtensions
    {
        public static Task<Result<Manager>> EnsureContractBelongsToContractManager(this Task<Result<Manager>> contractManager,
            DirectContractsDbContext dbContext, int contractId)
            => contractManager.Ensure(cm => dbContext.DoesContractBelongToContractManager(contractId, cm.Id),
                $"Invalid contract id '{contractId}'");


        public static Task<Result<Manager>> EnsureAccommodationBelongsToContractManager(this Task<Result<Manager>> contractManager,
            DirectContractsDbContext dbContext, int accommodationId)
            => contractManager.Ensure(
                async cm
                    => await dbContext.Accommodations.SingleOrDefaultAsync(a => a.Id == accommodationId && a.ContractManagerId == cm.Id) != null,
                $"Invalid accommodation id '{accommodationId}'");


        public static Task<Result<Manager>> EnsureRoomBelongsToContractManager(this Task<Result<Manager>> contractManager,
            DirectContractsDbContext dbContext, int accommodationId, int roomId)
            => contractManager
                .Ensure(
                async cm
                    => await dbContext.Rooms.SingleOrDefaultAsync(r => r.Id == roomId && r.AccommodationId == accommodationId) != null,
                $"Invalid room id '{roomId}'")
                .EnsureAccommodationBelongsToContractManager(dbContext, accommodationId);
    }
}