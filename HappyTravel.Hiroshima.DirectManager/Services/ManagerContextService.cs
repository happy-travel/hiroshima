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

        public async Task<Result<Manager>> GetContractManager()
        {
            var identityHash = GetIdentityHash();

            var contractManager = await _dbContext.Managers.SingleOrDefaultAsync(manager => manager.IsActive && manager.IdentityHash  == identityHash);

            return contractManager is null
                ? Result.Failure<Manager>("Failed to retrieve a contract manager") 
                : Result.Success(contractManager);
        }


        public async Task<bool> DoesContractManagerExist()
            => await _dbContext.Managers.SingleOrDefaultAsync(contractManager => contractManager.IdentityHash == GetIdentityHash()) != null;

        
        public string GetIdentityHash() => _tokenInfoAccessor.GetIdentityHash();
        
        
        private readonly DirectContractsDbContext _dbContext;
        private readonly ITokenInfoAccessor _tokenInfoAccessor;
    }
}