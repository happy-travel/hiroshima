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
        public static Task<Result<ServiceSupplier>> EnsureAccommodationBelongsToCompany(this Task<Result<ServiceSupplier>> serviceSupplier,
            DirectContractsDbContext dbContext, int accommodationId)
            => serviceSupplier.Ensure(c => dbContext.DoesAccommodationBelongToCompany(accommodationId, c.Id),
                $"Invalid accommodation id '{accommodationId}'");


        public static Task<Result<ServiceSupplier>> EnsureContractBelongsToCompany(this Task<Result<ServiceSupplier>> serviceSupplier,
            DirectContractsDbContext dbContext, int contractId)
            => serviceSupplier.Ensure(c => dbContext.DoesContractBelongToCompany(contractId, c.Id),
                $"Invalid contract id '{contractId}'");


        public static Task<Result<ServiceSupplier>> EnsureRoomBelongsToCompany(this Task<Result<ServiceSupplier>> serviceSupplier,
            DirectContractsDbContext dbContext, int accommodationId, int roomId)
            => serviceSupplier
                .Ensure(async serviceSupplier => await dbContext.Rooms.SingleOrDefaultAsync(r => r.Id == roomId && r.AccommodationId == accommodationId) != null,
                    $"Invalid room id '{roomId}'")
                .EnsureAccommodationBelongsToCompany(dbContext, accommodationId);
    }
}
