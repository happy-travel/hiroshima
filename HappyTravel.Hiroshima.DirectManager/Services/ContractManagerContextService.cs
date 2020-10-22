using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.DirectManager.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public class ContractManagerContextService : IContractManagerContextService
    {
        public ContractManagerContextService(DirectContractsDbContext dbContext, ITokenInfoAccessor tokenInfoAccessor)
        {
            _dbContext = dbContext;
            _tokenInfoAccessor = tokenInfoAccessor;
        }

        public async Task<Result<ContractManager>> GetContractManager()
        {
            var identityHash = GetIdentityHash();

            //var contractManager = await _dbContext.ContractManagers.SingleOrDefaultAsync(manager => manager.IsActive && manager.IdentityHash  == identityHash);
            var contractManager = await _dbContext.ContractManagers.SingleOrDefaultAsync(manager => manager.Id == 5);

            return contractManager is null
                ? Result.Failure<ContractManager>("Failed to retrieve a contract manager") 
                : Result.Success(contractManager);
        }


        public async Task<bool> DoesContractManagerExist()
        //    => await _dbContext.ContractManagers.SingleOrDefaultAsync(contractManager => contractManager.IdentityHash == GetIdentityHash()) != null;
        => true;
        
        public string GetIdentityHash() => _tokenInfoAccessor.GetIdentityHash();
        
        
        private readonly DirectContractsDbContext _dbContext;
        private readonly ITokenInfoAccessor _tokenInfoAccessor;
    }
}