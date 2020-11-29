using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Data;
using Microsoft.EntityFrameworkCore;

namespace HappyTravel.Hiroshima.DirectManager.Infrastructure.Extensions
{
    public static class ManagerExtensions
    {
        public static async Task<Result<Company>> GetCompany(this Task<Result<Manager>> manager, DirectContractsDbContext dbContext)
        {
            var company = await dbContext.Companies.SingleOrDefaultAsync(company => company.Id == manager.Result.Value.CompanyId);

            return company is null
                ? Result.Failure<Company>("Failed to retrieve a company")
                : Result.Success(company);
        }
    }
}