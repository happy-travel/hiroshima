using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public class ContractManagerContextService : IContractManagerContextService
    {
        public ContractManagerContextService(DirectContractsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        
        public async Task<Result<ContractManager>> GetContractManager()
        {
            //int.MaxValue is temporary ID. 
            //TODO add auth 
            var user = await _dbContext.ContractManagers.SingleOrDefaultAsync(cm => cm.Id  == int.MaxValue);
            
            return user.Equals(default) 
                ? Result.Failure<ContractManager>("Failed to retrieve a contract manager") 
                : Result.Ok(user);
        }

        
        private readonly DirectContractsDbContext _dbContext;
    }
}