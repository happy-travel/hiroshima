using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.Data.Extensions;
using HappyTravel.Hiroshima.Data.Models;

namespace HappyTravel.Hiroshima.DirectManager.Infrastructure.Extensions
{
    public static class ContractManagerExtensions
    {
        public static Task<Result<ContractManager>> EnsureContractBelongsToContractManager(this Task<Result<ContractManager>> contractedManager, DirectContractsDbContext dbContext, int contractId)
            => contractedManager.Ensure(contractManager => dbContext.DoesContractBelongToContractManager(contractId, contractManager.Id),
                $"Contract '{contractId}' doesn't belong to the contract manager");
    }
}