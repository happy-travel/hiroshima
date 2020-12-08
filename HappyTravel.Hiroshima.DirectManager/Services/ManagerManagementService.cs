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
        public ManagerManagementService(IManagerContextService managerContextService, IServiceSupplierContextService serviceSupplierContextService,
            DirectContractsDbContext dbContext)
        {
            _managerContext = managerContextService;
            _serviceSupplierContext = serviceSupplierContextService;
            _dbContext = dbContext;
        }


        public Task<Result<Models.Responses.ManagerContext>> Get()
            => _managerContext.GetManager()
                .Map(Build);


        public Task<Result<Models.Responses.ManagerContext>> Get(int managerId)
        {
            return _managerContext.GetServiceSupplier()
                //.Check(serviceSupplier => _serviceSupplierContext.EnsureManagerBelongsToServiceSupplier(managerId))
                .Map(company => GetManagerById(company.Id, managerId))
                .Map(Build);


            async Task<Common.Models.Manager> GetManagerById(int companyId, int managerId)
            {
                return await _dbContext.Managers.SingleOrDefaultAsync(manager => manager.Id == managerId);
            }
        }

        public Task<Result<Models.Responses.ManagerContext>> Register(Models.Requests.Manager managerRequest, string email)
        {
           return CheckIdentityHashNotEmpty()
                .Ensure(DoesManagerNotExist, "Manager has already been registered")
                .Bind(() => IsRequestValid(managerRequest))
                .Map(Create)
                .Map(Add)
                .Map(Build);

            
            Result CheckIdentityHashNotEmpty()
            {
                return string.IsNullOrEmpty(_managerContext.GetIdentityHash())
                    ? Result.Failure("Failed to get the sub claim")
                    : Result.Success();
            }
            
            
            async Task<bool> DoesManagerNotExist() => !await _managerContext.DoesManagerExist();


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
                var entry = _dbContext.Managers.Add(manager);
                await _dbContext.SaveChangesAsync();
                _dbContext.DetachEntry(entry.Entity);

                return entry.Entity;
            }
        }


        public Task<Result<Models.Responses.ServiceSupplier>> RegisterServiceSupplier(Models.Requests.ServiceSupplier serviceSupplierRequest)
        {
            return _managerContext.GetManager()
                .Check(manager => IsRequestValid(serviceSupplierRequest))
                .Bind(AddServiceSupplierAndRelation)
                .Map(Build);


            async Task<Result<Common.Models.ServiceSupplier>> AddServiceSupplierAndRelation(Common.Models.Manager manager)
            {
                return await Result.Success()
                    .Bind(CreateServiceSupplier)
                    .Bind(async serviceSupplier => await AddServiceSupplier(serviceSupplier))
                    .Tap(AddRelation);


                Result<Common.Models.ServiceSupplier> CreateServiceSupplier()
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

                async Task<Result<Common.Models.ServiceSupplier>> AddServiceSupplier(Common.Models.ServiceSupplier serviceSupplier)
                {
                    var entry = _dbContext.ServiceSuppliers.Add(serviceSupplier);
                    await _dbContext.SaveChangesAsync();
                    _dbContext.DetachEntry(entry.Entity);

                    return entry.Entity;
                }

                async Task AddRelation(Common.Models.ServiceSupplier serviceSupplier)
                {
                    var relation = new Common.Models.ManagerServiceSupplierRelation
                    {
                        ManagerId = manager.Id,
                        ManagerPermissions = Common.Models.Enums.ManagerPermissions.All,
                        ServiceSupplierId = serviceSupplier.Id,
                        IsMaster = true,
                        IsActive = true
                    };

                    var entry = _dbContext.ManagerServiceSupplierRelations.Add(relation);
                    await _dbContext.SaveChangesAsync();
                    _dbContext.DetachEntry(entry.Entity);
                }
            }
        }


        public Task<Result<Models.Responses.ManagerContext>> Modify(Models.Requests.Manager managerRequest)
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


        public Task<Result<Models.Responses.ManagerContext>> ModifyPermissions(int managerId, Models.Requests.ManagerPermissions managerPermissionsRequest)
        {
            throw new NotImplementedException();
        }


        private Models.Responses.ManagerContext Build(Common.Models.Manager manager) 
            => new Models.Responses.ManagerContext(manager.FirstName, 
                manager.LastName, 
                manager.Title, 
                manager.Position,
                manager.Email,
                manager.Phone,
                manager.Fax,
                1,  // TODO: Now we have only one service supplier. Will be changed in the next task
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
            => ValidationHelper.Validate(managerRequest, new ManagerValidator());

        private Result IsRequestValid(Models.Requests.ManagerPermissions managerPermissionsRequest)
            => ValidationHelper.Validate(managerPermissionsRequest, new ManagerPermissionsRequestValidator());


        private readonly IManagerContextService _managerContext;
        private readonly IServiceSupplierContextService _serviceSupplierContext;
        private readonly DirectContractsDbContext _dbContext;
    }
}