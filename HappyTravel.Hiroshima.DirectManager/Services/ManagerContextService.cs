using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.DirectManager.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public class ManagerContextService : IManagerContextService
    {
        public ManagerContextService(DirectContractsDbContext dbContext, ITokenInfoAccessor tokenInfoAccessor)
        {
            _dbContext = dbContext;
            _tokenInfoAccessor = tokenInfoAccessor;
        }


        public async Task<Result<Manager>> GetManager()
        {
            var identityHash = GetIdentityHash();

            var manager = await _dbContext.Managers.SingleOrDefaultAsync(manager => manager.IsActive && manager.IdentityHash  == identityHash);

            return manager is null
                ? Result.Failure<Manager>("Failed to retrieve a contract manager") 
                : Result.Success(manager);
        }


        public async Task<bool> DoesManagerExist()
            => await _dbContext.Managers.SingleOrDefaultAsync(manager => manager.IdentityHash == GetIdentityHash()) != null;

        
        public string GetIdentityHash() => _tokenInfoAccessor.GetIdentityHash();


        public async Task<Result<ServiceSupplier>> GetServiceSupplier()
        {
            var manager = GetManager();
            if (manager.Result.IsFailure)
                return Result.Failure<ServiceSupplier>(manager.Result.Error);

            var serviceSupplier = await _dbContext.Companies.SingleOrDefaultAsync(serviceSupplier => serviceSupplier.Id == manager.Result.Value.ServiceSupplierId);

            return serviceSupplier is null
                ? Result.Failure<ServiceSupplier>("Failed to retrieve a service supplier")
                : Result.Success(serviceSupplier);
        }


        private readonly DirectContractsDbContext _dbContext;
        private readonly ITokenInfoAccessor _tokenInfoAccessor;
    }
}