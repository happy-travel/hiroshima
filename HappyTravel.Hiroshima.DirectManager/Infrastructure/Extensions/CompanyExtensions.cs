using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HappyTravel.Hiroshima.DirectManager.Infrastructure.Extensions
{
    public static class CompanyExtensions
    {
        public static Task<Result<Company>> EnsureAccommodationBelongsToCompany(this Task<Result<Company>> company,
            DirectContractsDbContext dbContext, int accommodationId)
            => company.Ensure(c => dbContext.DoesAccommodationBelongToCompany(accommodationId, c.Id),
                $"Invalid accommodation id '{accommodationId}'");


        public static Task<Result<Company>> EnsureContractBelongsToCompany(this Task<Result<Company>> company,
            DirectContractsDbContext dbContext, int contractId)
            => company.Ensure(c => dbContext.DoesContractBelongToCompany(contractId, c.Id),
                $"Invalid contract id '{contractId}'");


        public static Task<Result<Company>> EnsureManagerBelongsToCompany(this Task<Result<Company>> company,
            DirectContractsDbContext dbContext, int managerId)
            => company.Ensure(c => dbContext.DoesManagerBelongToCompany(managerId, c.Id),
                $"Invalid manager id '{managerId}'");


        public static Task<Result<Company>> EnsureRoomBelongsToCompany(this Task<Result<Company>> company,
            DirectContractsDbContext dbContext, int accommodationId, int roomId)
            => company
                .Ensure(async c => await dbContext.Rooms.SingleOrDefaultAsync(r => r.Id == roomId && r.AccommodationId == accommodationId) != null,
                    $"Invalid room id '{roomId}'")
                .EnsureAccommodationBelongsToCompany(dbContext, accommodationId);
    }
}
