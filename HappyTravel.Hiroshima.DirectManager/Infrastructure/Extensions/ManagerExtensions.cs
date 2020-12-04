using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Data;
using Microsoft.EntityFrameworkCore;

namespace HappyTravel.Hiroshima.DirectManager.Infrastructure.Extensions
{
    public static class ManagerExtensions
    {
        public static async Task<Result<ServiceSupplier>> GetCompany(this Task<Result<Manager>> manager, DirectContractsDbContext dbContext)
        {
            var serviceSupplier = await dbContext.Companies.SingleOrDefaultAsync(serviceSupplier => serviceSupplier.Id == manager.Result.Value.ServiceSupplierId);

            return serviceSupplier is null
                ? Result.Failure<ServiceSupplier>("Failed to retrieve a service supplier")
                : Result.Success(serviceSupplier);
        }
    }
}