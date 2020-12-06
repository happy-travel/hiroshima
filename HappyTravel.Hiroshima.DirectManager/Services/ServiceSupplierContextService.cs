using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public class ServiceSupplierContextService : IServiceSupplierContextService
    {
        public ServiceSupplierContextService(DirectContractsDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public Result<ServiceSupplier> EnsureContractBelongsToServiceSupplier(ServiceSupplier serviceSupplier, int contractId)
        {
            return _dbContext.DoesContractBelongToServiceSupplier(contractId, serviceSupplier.Id).Result
                ? Result.Success(serviceSupplier)
                : Result.Failure<ServiceSupplier>($"Invalid contract id '{contractId}'");
        }


        private readonly DirectContractsDbContext _dbContext;
    }
}
