using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Common.Models.Enums;
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.Data.Extensions;
using HappyTravel.Hiroshima.DirectContracts.Extensions.FunctionalExtensions;
using HappyTravel.Hiroshima.DirectManager.Infrastructure.Logging;
using HappyTravel.Hiroshima.DirectManager.RequestValidators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public class ManagerRegistrationService : IManagerRegistrationService
    {
        public ManagerRegistrationService(IManagerContextService managerContextService, IManagerInvitationService managerInvitationService, 
            INotificationService notificationService, DirectContractsDbContext dbContext, ILogger<ManagerRegistrationService> logger)
        {
            _managerContext = managerContextService;
            _managerInvitationService = managerInvitationService;
            _notificationService = notificationService;
            _dbContext = dbContext;
            _logger = logger;
        }


        public Task<Result<Models.Responses.ManagerContext>> RegisterWithServiceSupplier(Models.Requests.ManagerWithServiceSupplier managerRequest, string email)
        {
            return CheckIdentityHashNotEmpty()
                 .Ensure(DoesManagerNotExist, "Manager has already been registered")
                 .Bind(() => ValidateRequest(managerRequest))
                 .Bind(AddManager)
                 .Bind(AddServiceSupplierAndRelation)
                 .Map(Build);


            async Task<bool> DoesManagerNotExist() => !await _managerContext.DoesManagerExist();


            async Task<Result<Manager>> AddManager()
            {
                return await Create()
                    .Bind(Add);


                Result<Manager> Create()
                {
                    var utcNowDate = DateTime.UtcNow;
                    return new Manager
                    {
                        IdentityHash = _managerContext.GetIdentityHash(),
                        Email = email,
                        FirstName = managerRequest.Manager.FirstName,
                        LastName = managerRequest.Manager.LastName,
                        Title = managerRequest.Manager.Title,
                        Position = managerRequest.Manager.Position,
                        Phone = managerRequest.Manager.Phone,
                        Fax = managerRequest.Manager.Fax,
                        Created = utcNowDate,
                        Updated = utcNowDate,
                        IsActive = true
                    };
                }


                async Task<Result<Manager>> Add(Manager manager)
                {
                    var entry = _dbContext.Managers.Add(manager);
                    await _dbContext.SaveChangesAsync();
                    _dbContext.DetachEntry(entry.Entity);

                    return entry.Entity;
                }
            }


            async Task<Result<ManagerContext>> AddServiceSupplierAndRelation(Manager manager)
            {
                return await CreateServiceSupplier()
                    .Bind(AddServiceSupplier)
                    .Bind(serviceSupplier => CreateRelation(serviceSupplier))
                    .Bind(AddRelation);


                Result<ServiceSupplier> CreateServiceSupplier()
                {
                    var utcNowDate = DateTime.UtcNow;
                    return new ServiceSupplier
                    {
                        Name = managerRequest.ServiceSupplier.Name,
                        Address = managerRequest.ServiceSupplier.Address,
                        PostalCode = managerRequest.ServiceSupplier.PostalCode,
                        Phone = managerRequest.ServiceSupplier.Phone,
                        Website = managerRequest.ServiceSupplier.Website,
                        Created = utcNowDate,
                        Modified = utcNowDate
                    };
                }


                async Task<Result<ServiceSupplier>> AddServiceSupplier(ServiceSupplier serviceSupplier)
                {
                    var entry = _dbContext.ServiceSuppliers.Add(serviceSupplier);
                    await _dbContext.SaveChangesAsync();
                    _dbContext.DetachEntry(entry.Entity);

                    return entry.Entity;
                }


                Result<ManagerServiceSupplierRelation> CreateRelation(ServiceSupplier serviceSupplier) 
                    => new ManagerServiceSupplierRelation
                    {
                        ManagerId = manager.Id,
                        ManagerPermissions = ManagerPermissions.All,
                        ServiceSupplierId = serviceSupplier.Id,
                        IsMaster = true,
                        IsActive = true
                    };


                async Task<Result<ManagerContext>> AddRelation(ManagerServiceSupplierRelation managerRelation)
                {
                    var entry = _dbContext.ManagerServiceSupplierRelations.Add(managerRelation);
                    await _dbContext.SaveChangesAsync();
                    _dbContext.DetachEntry(entry.Entity);

                    return CollectManagerContext(manager, entry.Entity);
                }
            }
        }


        public Task<Result<Models.Responses.ManagerContext>> RegisterInvited(Models.Requests.ManagerInfoWithCode managerInfoRequest, string email)
        {
            return CheckIdentityHashNotEmpty()
                .Bind(GetPendingInvitation)
                .Ensure(IsEmailUnique, "Manager with this email already exists")
                .Check(managerInvitation => ValidateRequest(managerInfoRequest))
                .BindWithTransaction(_dbContext, managerInvitation => Result.Success(managerInvitation)
                    .Bind(AddManager)
                    .Bind(AddManagerRelation)
                    .Tap(AcceptInvitation))
                .Tap(LogSuccess)
                .Bind(GetMasterManager)
                .Bind(SendRegistrationMailToMaster)
                .OnFailure(LogFailed)
                .Map(Build);


            Task<Result<Models.Responses.ManagerInvitation>> GetPendingInvitation() => _managerInvitationService.GetPendingInvitation(managerInfoRequest.InvitationCode);


            async Task<bool> IsEmailUnique(Models.Responses.ManagerInvitation managerInvitation)
                => !await _dbContext.Managers.AnyAsync(manager => manager.Email == managerInvitation.Email);



            async Task<Result<(Models.Responses.ManagerInvitation, Manager)>> AddManager(Models.Responses.ManagerInvitation managerInvitation)
            {
                var utcNowDate = DateTime.UtcNow;
                var manager = new Manager
                {
                    IdentityHash = _managerContext.GetIdentityHash(),
                    Email = email,
                    FirstName = managerInfoRequest.FirstName,
                    LastName = managerInfoRequest.LastName,
                    Title = managerInfoRequest.Title,
                    Position = managerInfoRequest.Position,
                    Created = utcNowDate,
                    Updated = utcNowDate,
                    IsActive = true
                };

                _dbContext.Managers.Add(manager);
                await _dbContext.SaveChangesAsync();

                return Result.Success((managerInvitation, manager));
            }


            async Task<Result<ManagerContext>> AddManagerRelation((Models.Responses.ManagerInvitation, Manager) invitationData)
            {
                var (managerInvitation, manager) = invitationData;

                var managerRelation = await AddManagerServiceSupplierRelation(manager, ManagerPermissions.All, false, managerInvitation.ServiceSupplierId);

                var managerContext = CollectManagerContext(manager, managerRelation);

                return Result.Success(managerContext);
            }


            async Task AcceptInvitation() => await _managerInvitationService.Accept(managerInfoRequest.InvitationCode);


            void LogSuccess(ManagerContext managerContext) 
                => _logger.LogManagerRegistrationSuccess($"Manager with the {nameof(email)} '{email}' was successfully registered and was bound to the service supplier ID:'{managerContext.ServiceSupplierId}'");


            async Task<Result<(ManagerContext, Manager)>> GetMasterManager(ManagerContext managerContext)
            {
                var masterManager = await _managerContext.GetMasterManager(managerContext.ServiceSupplierId);
                if (masterManager.IsFailure)
                    return Result.Failure<(ManagerContext, Manager)>(masterManager.Error);

                return Result.Success((managerContext, masterManager.Value));
            }


            async Task<Result<ManagerContext>> SendRegistrationMailToMaster((ManagerContext, Manager) registrationData)
            {
                var (managerContext, masterManager) = registrationData;

                var serviceSupplier = await _dbContext.ServiceSuppliers.SingleOrDefaultAsync(serviceSupplier => serviceSupplier.Id == managerContext.ServiceSupplierId); 
                if (serviceSupplier is null)
                    return Result.Failure<ManagerContext>("Service supplier not found");

                var sendResult = await _notificationService.SendRegistrationConfirmation(masterManager.Email, managerInfoRequest, serviceSupplier.Name);
                if (sendResult.IsFailure)
                    return Result.Failure<ManagerContext>(sendResult.Error);

                return Result.Success(managerContext);
            }


            void LogFailed(string error) => _logger.LogManagerRegistrationFailed(error);
        }


        private Result CheckIdentityHashNotEmpty()
        {
            return string.IsNullOrEmpty(_managerContext.GetIdentityHash())
                ? Result.Failure("Manager should have identity")
                : Result.Success();
        }


        private Result ValidateRequest(Models.Requests.ManagerWithServiceSupplier managerRequest)
            => ValidationHelper.Validate(managerRequest, new ManagerWithServiceSupplierValidator());


        private Result ValidateRequest(Models.Requests.ManagerInfoWithCode managerInfoRequest)
            => ValidationHelper.Validate(managerInfoRequest, new ManagerInfoWithCodeValidator());


        private async Task<ManagerServiceSupplierRelation> AddManagerServiceSupplierRelation(Manager manager, ManagerPermissions managerPermissions, bool isMaster, int serviceSupplierId)
        {
            var entry =_dbContext.ManagerServiceSupplierRelations.Add(new ManagerServiceSupplierRelation
            {
                ManagerId = manager.Id,
                ManagerPermissions = managerPermissions,
                IsMaster = isMaster,
                ServiceSupplierId = serviceSupplierId,
                IsActive = true
            });
            await _dbContext.SaveChangesAsync();
            _dbContext.DetachEntry(entry.Entity);

            return entry.Entity;
        }


        private static ManagerContext CollectManagerContext(Manager manager, ManagerServiceSupplierRelation managerRelation) 
            => new ManagerContext
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


        private static Models.Responses.ManagerContext Build(ManagerContext managerContext)
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


        private readonly IManagerContextService _managerContext;
        private readonly IManagerInvitationService _managerInvitationService;
        private readonly INotificationService _notificationService;
        private readonly DirectContractsDbContext _dbContext;
        private readonly ILogger<ManagerRegistrationService> _logger;
    }
}