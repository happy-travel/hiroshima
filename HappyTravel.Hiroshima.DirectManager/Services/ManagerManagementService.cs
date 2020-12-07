using System;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.Data.Extensions;
using HappyTravel.Hiroshima.DirectManager.RequestValidators;
using Microsoft.EntityFrameworkCore;

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
                .Ensure(DoesManagerNotExist, "Manager has already been registered")
                .Bind(() => IsRequestValid(managerRequest))
                .Map(CreateServiceSupplier)
                .Map(AddServiceSupplier)
                .Map(Create)
                .Map(Add)
                .Map(Build);

            
            bool IdentityHashNotEmpty() => !string.IsNullOrEmpty(_managerContext.GetIdentityHash());
            
            
            async Task<bool> DoesManagerNotExist() => !await _managerContext.DoesManagerExist();


            Common.Models.ServiceSupplier CreateServiceSupplier()
            {
                var utcNowDate = DateTime.UtcNow;
                return new Common.Models.ServiceSupplier
                {
                    Name = string.Empty,
                    Address = string.Empty,
                    PostalCode = string.Empty,
                    Phone = string.Empty,
                    Website = string.Empty,
                    Created = utcNowDate,
                    Modified = utcNowDate
                };
            }


            async Task<Common.Models.ServiceSupplier> AddServiceSupplier(Common.Models.ServiceSupplier serviceSupplier)
            {
                var entry = _dbContext.ServiceSuppliers.Add(serviceSupplier);
                await _dbContext.SaveChangesAsync();
                _dbContext.DetachEntry(entry.Entity);

                return entry.Entity;
            }


            Common.Models.Manager Create(Common.Models.ServiceSupplier serviceSupplier)
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
                var entry = _dbContext.Managers.Add(manager);
                await _dbContext.SaveChangesAsync();
                _dbContext.DetachEntry(entry.Entity);

                return entry.Entity;
            }
        }


        public Task<Result<Models.Responses.ServiceSupplier>> RegisterServiceSupplier(Models.Requests.ServiceSupplier serviceSupplierRequest)
        {
            return _managerContext.GetManager()
                .Bind(GetRelation)
                .Check(relation => IsRequestValid(serviceSupplierRequest))
                .Bind(ModifyServiceSupplier)
                .Map(Update)
                .Map(Build);


            async Task<Result<Common.Models.ManagerServiceSupplierRelation>> GetRelation(Common.Models.Manager manager)
            {
                // TODO: Now it is assumed that the manager only works in one company. Will be changed in the next tasks
                var managerRelation = await _dbContext.ManagerServiceSupplierRelations
                    .SingleOrDefaultAsync(relation => relation.ManagerId == manager.Id && relation.IsActive && relation.IsMaster);

                return managerRelation is null
                    ? Result.Failure<Common.Models.ManagerServiceSupplierRelation>("Manager has no rights to register the service supplier")
                    : Result.Success(managerRelation);
            }


            async Task<Result<Common.Models.ServiceSupplier>> ModifyServiceSupplier(Common.Models.ManagerServiceSupplierRelation managerRelation)
            {
                var serviceSupplier = await _dbContext.ServiceSuppliers.SingleOrDefaultAsync(serviceSupplier => serviceSupplier.Id == managerRelation.ServiceSupplierId);
                if (serviceSupplier is null)
                    return Result.Failure<Common.Models.ServiceSupplier>("Invalid service supplier");

                serviceSupplier.Name = serviceSupplierRequest.Name;
                serviceSupplier.Address = serviceSupplierRequest.Address;
                serviceSupplier.PostalCode = serviceSupplierRequest.PostalCode;
                serviceSupplier.Phone = serviceSupplierRequest.Phone;
                serviceSupplier.Website = serviceSupplierRequest.Website;
                serviceSupplier.Modified = DateTime.UtcNow;

                return serviceSupplier;
            }


            async Task<Common.Models.ServiceSupplier> Update(Common.Models.ServiceSupplier serviceSupplier)
            {
                var entry = _dbContext.ServiceSuppliers.Update(serviceSupplier);
                await _dbContext.SaveChangesAsync();
                _dbContext.DetachEntry(entry.Entity);

                return entry.Entity;
            }
        }


        public Task<Result<Models.Responses.Manager>> Modify(Models.Requests.Manager managerRequest)
        {
            return GetManager()
                .Check(manager => IsRequestValid(managerRequest))
                .Map(ModifyManager)
                .Map(Update)
                .Map(Build);

            
            Task<Result<Common.Models.Manager>> GetManager() 
                => _managerContext.GetManager();
            
            
            Common.Models.Manager ModifyManager(Common.Models.Manager manager)
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
                manager.Fax,
                Common.Models.Enums.ManagerPermissions.All, // TODO: Need add ManagerPermissions and IsMaster in next task
                true);


        private Models.Responses.ServiceSupplier Build(Common.Models.ServiceSupplier serviceSupplier)
            => new Models.Responses.ServiceSupplier(serviceSupplier.Name,
                serviceSupplier.Address,
                serviceSupplier.PostalCode,
                serviceSupplier.Phone,
                serviceSupplier.Website);


        private Result IsRequestValid(Models.Requests.ServiceSupplier serviceSupplierRequest)
            => ValidationHelper.Validate(serviceSupplierRequest, new ServiceSupplierValidator());


        private Result IsRequestValid(Models.Requests.Manager managerRequest)
            => ValidationHelper.Validate(managerRequest, new ManagerRegisterRequestValidator());


        private readonly IManagerContextService _managerContext;
        private readonly DirectContractsDbContext _dbContext;
    }
}