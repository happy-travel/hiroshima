using System;
using System.Collections.Generic;
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


        public Task<Result<Models.Responses.ManagerContext>> Get()
            => _managerContext.GetManagerContext()
                .Map(Build);


        public Task<Result<Models.Responses.ManagerContext>> Get(int managerId)
        {
            return _managerContext.GetManagerRelation()
                .Ensure(managerRelation => HasManagerChangeManagerPermission(managerRelation).Value, "The manager does not have enough rights")
                .Bind(managerRelation => GetManagerContextById(managerId, managerRelation.ServiceSupplierId))
                .Map(Build);


            async Task<Result<Common.Models.ManagerContext>> GetManagerContextById(int managerId, int serviceSupplierId)
            {
                var managerRelation = await _dbContext.ManagerServiceSupplierRelations
                    .SingleOrDefaultAsync(relation => relation.ManagerId == managerId && relation.ServiceSupplierId == serviceSupplierId);
                if (managerRelation is null)
                    return Result.Failure<Common.Models.ManagerContext>($"Manager with ID {managerId} has no relation with service supplier");

                var manager = await _dbContext.Managers.SingleOrDefaultAsync(manager => manager.Id == managerId);
                if (manager is null)
                    return Result.Failure<Common.Models.ManagerContext>($"Manager with ID {managerId} not found");

                return CollectManagerContext(manager, managerRelation);
            }
        }


        public Task<Result<List<Models.Responses.ServiceSupplier>>> GetServiceSuppliers()
        {
            return _managerContext.GetManager()
                .Bind(Get)
                .Map(Build);

            async Task<Result<List<Common.Models.ServiceSupplier>>> Get(Common.Models.Manager manager)
            {
                var managerRelations = await _dbContext.ManagerServiceSupplierRelations
                    .Where(relation => relation.ManagerId == manager.Id)
                    .Select(relation => relation.ServiceSupplierId)
                    .ToListAsync();
                if (!managerRelations.Any())
                    return Result.Failure<List<Common.Models.ServiceSupplier>>("Manager relations not found");

                var serviceSuppliers = await _dbContext.ServiceSuppliers
                    .Join(managerRelations, ss => ss.Id, r => r, 
                        (ss, r) => new Common.Models.ServiceSupplier { Id = ss.Id, Name = ss.Name, Address = ss.Address, PostalCode = ss.PostalCode, Phone = ss.Phone, Website = ss.Website })
                    .ToListAsync();
                if (!serviceSuppliers.Any())
                    return Result.Failure<List<Common.Models.ServiceSupplier>>("Service suppliers not found");

                return Result.Success(serviceSuppliers);
            }
        }


        public Task<Result<Models.Responses.ManagerContext>> Modify(Models.Requests.Manager managerRequest)
        {
            return _managerContext.GetManager()
                .Check(manager => ValidateRequest(managerRequest))
                .Bind(Update)
                .Map(Build);
            
            
            async Task<Result<Common.Models.ManagerContext>> Update(Common.Models.Manager manager)
            {
                manager.FirstName = managerRequest.FirstName;
                manager.LastName = managerRequest.LastName;
                manager.Title = managerRequest.Title;
                manager.Position = managerRequest.Position;
                manager.Phone = managerRequest.Phone;
                manager.Fax = managerRequest.Fax;
                manager.Updated = DateTime.UtcNow;

                var entry = _dbContext.Managers.Update(manager);
                await _dbContext.SaveChangesAsync();
                _dbContext.DetachEntry(entry.Entity);

                var managerRelation = await _managerContext.GetManagerRelation();
                if (managerRelation.IsFailure)
                    return Result.Failure<Common.Models.ManagerContext>(managerRelation.Error);

                return CollectManagerContext(manager, managerRelation.Value);
            }
        }


        public Task<Result<Models.Responses.ManagerContext>> ModifyPermissions(int managerId, Models.Requests.Permissions permissionsRequest)
        {
            return _managerContext.GetManagerRelation()
                .Ensure(managerRelation => HasManagerChangeManagerPermission(managerRelation).Value, "The manager does not have enough rights")
                .Check(managerRelation => ValidateRequest(permissionsRequest))
                .Bind(managerRelation => UpdatePermissions(managerRelation.ServiceSupplierId))
                .Map(Build);


            async Task<Result<Common.Models.ManagerContext>> UpdatePermissions(int serviceSupplierId)
            {
                var managerRelation = await _dbContext.ManagerServiceSupplierRelations
                    .SingleOrDefaultAsync(relation => relation.ManagerId == managerId && relation.ServiceSupplierId == serviceSupplierId);
                if (managerRelation is null)
                    return Result.Failure<Common.Models.ManagerContext>($"Manager with ID {managerId} has no relation with service supplier");

                var manager = await _dbContext.Managers.SingleOrDefaultAsync(manager => manager.Id == managerId);
                if (manager is null)
                    return Result.Failure<Common.Models.ManagerContext>($"Manager with ID {managerId} not found");

                managerRelation.ManagerPermissions = permissionsRequest.ManagerPermissions;
                _dbContext.ManagerServiceSupplierRelations.Update(managerRelation);
                await _dbContext.SaveChangesAsync();

                var managerContext = CollectManagerContext(manager, managerRelation);

                return Result.Success(managerContext);
            }
        }


        public Task<Result<Models.Responses.ServiceSupplier>> ModifyServiceSupplier(Models.Requests.ServiceSupplier serviceSupplierRequest)
        {
            return _managerContext.GetManagerRelation()
                .Ensure(managerRelation => HasManagerModifyServiceSupplier(managerRelation).Value, "The manager does not have enough rights")
                .Check(managerRelation => ValidateRequest(serviceSupplierRequest))
                .Bind(Modify)
                .Map(Build);


            async Task<Result<Common.Models.ServiceSupplier>> Modify(Common.Models.ManagerServiceSupplierRelation managerRelation)
            {
                var serviceSupplier = await _dbContext.ServiceSuppliers
                    .SingleOrDefaultAsync(serviceSupplier => serviceSupplier.Id == managerRelation.ServiceSupplierId);

                serviceSupplier.Name = serviceSupplierRequest.Name;
                serviceSupplier.Address = serviceSupplierRequest.Address;
                serviceSupplier.PostalCode = serviceSupplierRequest.PostalCode;
                serviceSupplier.Phone = serviceSupplierRequest.Phone;
                serviceSupplier.Website = serviceSupplierRequest.Website;
                serviceSupplier.Modified = DateTime.UtcNow;

                _dbContext.ServiceSuppliers.Update(serviceSupplier);
                await _dbContext.SaveChangesAsync();

                return serviceSupplier;
            }
        }


        private Common.Models.ManagerContext CollectManagerContext(Common.Models.Manager manager, Common.Models.ManagerServiceSupplierRelation managerRelation) 
            => new Common.Models.ManagerContext
            {
                Id = manager.Id,
                FirstName = manager.FirstName,
                LastName = manager.LastName,
                Title = manager.Title,
                Position = manager.Position,
                Email = manager.Email,
                Phone = manager.Phone,
                Fax = manager.Fax,
                ServiceSupplierId = managerRelation.ServiceSupplierId,
                ManagerPermissions = managerRelation.ManagerPermissions,
                IsMaster = managerRelation.IsMaster
            };


        private static Models.Responses.ManagerContext Build(Common.Models.ManagerContext managerContext) 
            => new Models.Responses.ManagerContext(managerContext.FirstName,
                managerContext.LastName,
                managerContext.Title,
                managerContext.Position,
                managerContext.Email,
                managerContext.Phone,
                managerContext.Fax,
                managerContext.ServiceSupplierId,
                managerContext.ManagerPermissions,
                managerContext.IsMaster);


        private static Models.Responses.ServiceSupplier Build(Common.Models.ServiceSupplier serviceSupplier)
            => new Models.Responses.ServiceSupplier(serviceSupplier.Name,
                serviceSupplier.Address,
                serviceSupplier.PostalCode,
                serviceSupplier.Phone,
                serviceSupplier.Website);


        private static List<Models.Responses.ServiceSupplier> Build(List<Common.Models.ServiceSupplier> serviceSuppliers)
            => serviceSuppliers.Select(Build).ToList();


        private Result<bool> HasManagerChangeManagerPermission(Common.Models.ManagerServiceSupplierRelation managerRelation)
            => (managerRelation.ManagerPermissions & Common.Models.Enums.ManagerPermissions.ChangeManagerPermissions) == Common.Models.Enums.ManagerPermissions.ChangeManagerPermissions;


        private Result<bool> HasManagerModifyServiceSupplier(Common.Models.ManagerServiceSupplierRelation managerRelation)
            => managerRelation.IsMaster;


        private Result ValidateRequest(Models.Requests.Manager managerRequest)
            => ValidationHelper.Validate(managerRequest, new ManagerValidator());


        private Result ValidateRequest(Models.Requests.Permissions managerPermissionsRequest)
            => ValidationHelper.Validate(managerPermissionsRequest, new PermissionsRequestValidator());


        private Result ValidateRequest(Models.Requests.ServiceSupplier serviceSupplierRequest)
            => ValidationHelper.Validate(serviceSupplierRequest, new ServiceSupplierValidator());


        private readonly IManagerContextService _managerContext;
        private readonly DirectContractsDbContext _dbContext;
    }
}