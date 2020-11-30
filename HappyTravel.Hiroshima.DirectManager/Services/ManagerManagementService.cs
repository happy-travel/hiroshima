using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.Data.Extensions;
using HappyTravel.Hiroshima.DirectManager.RequestValidators;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public class ManagerManagementService : IManagerManagementService
    {
        public ManagerManagementService(IManagerContextService managerContextService, DirectContractsDbContext dbContext)
        {
            _managerContext = managerContextService;
            _dbContext = dbContext;
        }


        public Task<Result<Models.Responses.Manager>> Get()
            => _managerContext.GetManager()
                .Map(Build);
        
        
        public Task<Result<Models.Responses.Manager>> Register(Models.Requests.Manager managerRequest, string email)
        {
           return Result.Success()
                .Ensure(IdentityHashNotEmpty, "Failed to get the sub claim")
                .Ensure(ContractManagerNotExist, "Contract manager has already been registered")
                .Bind(() => IsRequestValid(managerRequest))
                .Map(Create)
                .Map(Add)
                .Map(Build);

            
            bool IdentityHashNotEmpty() => !string.IsNullOrEmpty(_managerContext.GetIdentityHash());
            
            
            async Task<bool> ContractManagerNotExist() => !await _managerContext.DoesManagerExist();
            
            
            Common.Models.Manager Create()
            {
                var utcNowDate = DateTime.UtcNow;
                return new Common.Models.Manager
                {
                    IdentityHash = _managerContext.GetIdentityHash(),
                    Email = email,
                    FirstName = managerRequest.FirstName,
                    LastName = managerRequest.LastName,
                    Title = managerRequest.Title,
                    Position = managerRequest.Position,
                    Phone = managerRequest.Phone,
                    Fax = managerRequest.Fax,
                    Created = utcNowDate,
                    Updated = utcNowDate,
                    IsActive = true
                };
            }


            async Task<Common.Models.Manager> Add(Common.Models.Manager manager)
            {
                var entry = _dbContext.Add(manager);
                await _dbContext.SaveChangesAsync();
                _dbContext.DetachEntry(entry.Entity);

                return entry.Entity;
            }
        }

        
        public Task<Result<Models.Responses.Manager>> Modify(Models.Requests.Manager managerRequest)
        {
            return GetManager()
                .Tap(manager => IsRequestValid(managerRequest))
                .Map(ModifyContractManager)
                .Map(Update)
                .Map(Build);

            
            Task<Result<Common.Models.Manager>> GetManager() 
                => _managerContext.GetManager();
            
            
            Common.Models.Manager ModifyContractManager(Common.Models.Manager manager)
            {
                manager.FirstName = managerRequest.FirstName;
                manager.LastName = managerRequest.LastName;
                manager.Title = managerRequest.Title;
                manager.Position = managerRequest.Position;
                manager.Phone = managerRequest.Phone;
                manager.Fax = managerRequest.Fax;
                manager.Updated = DateTime.UtcNow;

                return manager;
            }
            
            
            async Task<Common.Models.Manager> Update(Common.Models.Manager manager)
            {
                var entry = _dbContext.Managers.Update(manager);
                await _dbContext.SaveChangesAsync();
                _dbContext.DetachEntry(entry.Entity);
                
                return entry.Entity;
            }
        }

        
        private Models.Responses.Manager Build(Common.Models.Manager manager) 
            => new Models.Responses.Manager(manager.FirstName, 
                manager.LastName, 
                manager.Title, 
                manager.Position,
                manager.Email,
                manager.Phone,
                manager.Fax);


        private Result IsRequestValid(Models.Requests.Manager managerRequest)
            => ValidationHelper.Validate(managerRequest, new ManagerRegisterRequestValidator());
        

        private readonly IManagerContextService _managerContext;
        private readonly DirectContractsDbContext _dbContext;
    }
}