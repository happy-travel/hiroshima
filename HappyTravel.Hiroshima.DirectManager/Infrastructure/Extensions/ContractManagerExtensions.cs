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
        public static Task<Result<ContractManager>> EnsureContractBelongsToContractManager(this Task<Result<ContractManager>> contractManager,
            DirectContractsDbContext dbContext, int contractId)
            => contractManager.Ensure(cm => dbContext.DoesContractBelongToContractManager(contractId, cm.Id),
                $"Invalid contract id '{contractId}'");


        public static Task<Result<ContractManager>> EnsureAccommodationBelongsToContractManager(this Task<Result<ContractManager>> contractManager,
            DirectContractsDbContext dbContext, int accommodationId)
            => contractManager.Ensure(
                async cm
                    => await dbContext.Accommodations.SingleOrDefaultAsync(a => a.Id == accommodationId && a.ContractManagerId == cm.Id) != null,
                $"Invalid accommodation id '{accommodationId}'");


        public static Task<Result<ContractManager>> EnsureRoomBelongsToContractManager(this Task<Result<ContractManager>> contractManager,
            DirectContractsDbContext dbContext, int accommodationId, int roomId)
            => contractManager
                .Ensure(
                async cm
                    => await dbContext.Rooms.SingleOrDefaultAsync(r => r.Id == roomId && r.AccommodationId == accommodationId) != null,
                $"Invalid room id '{roomId}'")
                .EnsureAccommodationBelongsToContractManager(dbContext, accommodationId);
    }
}