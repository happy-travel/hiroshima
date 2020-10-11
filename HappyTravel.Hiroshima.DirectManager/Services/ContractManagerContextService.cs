using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Common.Models;
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
            var contractManager = await _dbContext.ContractManagers.SingleOrDefaultAsync(cm => cm.Id  == int.MaxValue);
            
            return contractManager is null
                ? Result.Failure<ContractManager>("Failed to retrieve a contract manager") 
                : Result.Success(contractManager);
        }


        private readonly DirectContractsDbContext _dbContext;
    }
}