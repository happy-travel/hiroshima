using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.DirectManager.Services;
using System;
using System.Threading.Tasks;

namespace HappyTravel.Hiroshima.DirectManager.UnitTests.Mocks
{
    public class ManagerContextServiceMock : IManagerContextService
    {
        public ManagerContextServiceMock(DirectContractsDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<Result<Manager>> GetManager()
        {
            throw new NotImplementedException();
        }


        public string GetHash()
        {
            throw new NotImplementedException();
        }


        public async Task<bool> DoesManagerExist()
        {
            throw new NotImplementedException();
        }


        public async Task<Result<ServiceSupplier>> GetServiceSupplier()
        {
            throw new NotImplementedException();
        }


        public async Task<Result<ManagerServiceSupplierRelation>> GetManagerRelation()
        {
            throw new NotImplementedException();
        }


        public async Task<Result<ManagerContext>> GetManagerContext()
        {
            throw new NotImplementedException();
        }


        public async Task<Result<Manager>> GetMasterManager(int serviceSupplierId)
        {
            throw new NotImplementedException();
        }


        private readonly DirectContractsDbContext _dbContext;
    }
}
