using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.Data.Extensions;
using HappyTravel.Hiroshima.DirectManager.RequestValidators;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public class ContractManagerManagementService : IContractManagerManagementService
    {
        public ContractManagerManagementService(IContractManagerContextService contractManagerContextService, DirectContractsDbContext dbContext)
        {
            _contractManagerContextService = contractManagerContextService;
            _dbContext = dbContext;
        }


        public Task<Result<Models.Responses.Manager>> Get()
            => _contractManagerContextService.GetContractManager()
                .Map(Build);
        
        
        public Task<Result<Models.Responses.Manager>> Register(Models.Requests.Manager contractManagerRequest, string email)
        {
           return Result.Success()
                .Ensure(IdentityHashNotEmpty, "Failed to get the sub claim")
                .Ensure(ContractManagerNotExist, "Contract manager has already been registered")
                .Bind(() => IsRequestValid(contractManagerRequest))
                .Map(Create)
                .Map(Add)
                .Map(Build);

            
            bool IdentityHashNotEmpty() => !string.IsNullOrEmpty(_contractManagerContextService.GetIdentityHash());
            
            
            async Task<bool> ContractManagerNotExist() => !await _contractManagerContextService.DoesContractManagerExist();
            
            
            Common.Models.Manager Create()
            {
                var utcNowDate = DateTime.UtcNow;
                return new Common.Models.Manager
                {
                    IdentityHash = _contractManagerContextService.GetIdentityHash(),
                    Email = email,
                    FirstName = contractManagerRequest.FirstName,
                    LastName = contractManagerRequest.LastName,
                    Title = contractManagerRequest.Title,
                    Position = contractManagerRequest.Position,
                    Phone = contractManagerRequest.Phone,
                    Fax = contractManagerRequest.Fax,
                    Created = utcNowDate,
                    Updated = utcNowDate,
                    IsActive = true
                };
            }


            async Task<Common.Models.Manager> Add(Common.Models.Manager contractManager)
            {
                var entry = _dbContext.Add(contractManager);
                await _dbContext.SaveChangesAsync();
                _dbContext.DetachEntry(entry.Entity);

                return entry.Entity;
            }
        }

        
        public Task<Result<Models.Responses.Manager>> Modify(Models.Requests.Manager contractManagerRequest)
        {
            return GetContractManager()
                .Tap(contractManager => IsRequestValid(contractManagerRequest))
                .Map(ModifyContractManager)
                .Map(Update)
                .Map(Build);

            
            Task<Result<Common.Models.Manager>> GetContractManager() 
                => _contractManagerContextService.GetContractManager();
            
            
            Common.Models.Manager ModifyContractManager(Common.Models.Manager contractManager)
            {
                contractManager.FirstName = contractManagerRequest.FirstName;
                contractManager.LastName = contractManagerRequest.LastName;
                contractManager.Title = contractManagerRequest.Title;
                contractManager.Position = contractManagerRequest.Position;
                contractManager.Phone = contractManagerRequest.Phone;
                contractManager.Fax = contractManagerRequest.Fax;
                contractManager.Updated = DateTime.UtcNow;

                return contractManager;
            }
            
            
            async Task<Common.Models.Manager> Update(Common.Models.Manager contractManager)
            {
                var entry = _dbContext.Managers.Update(contractManager);
                await _dbContext.SaveChangesAsync();
                _dbContext.DetachEntry(entry.Entity);
                
                return entry.Entity;
            }
        }

        
        private Models.Responses.Manager Build(Common.Models.Manager contractManager) 
            => new Models.Responses.Manager(contractManager.FirstName, 
                contractManager.LastName, 
                contractManager.Title, 
                contractManager.Position,
                contractManager.Email,
                contractManager.Phone,
                contractManager.Fax);


        private Result IsRequestValid(Models.Requests.Manager contractManagerRequest)
            => ValidationHelper.Validate(contractManagerRequest, new ContractManagerRegisterRequestValidator());
        

        private readonly IContractManagerContextService _contractManagerContextService;
        private readonly DirectContractsDbContext _dbContext;
    }
}