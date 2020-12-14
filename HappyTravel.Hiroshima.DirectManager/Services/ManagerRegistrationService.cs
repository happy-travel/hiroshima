using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Common.Models.Enums;
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.Data.Extensions;
using HappyTravel.Hiroshima.DirectManager.RequestValidators;
using HappyTravel.Hiroshima.WebApi.Infrastructure.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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
                 .Bind(() => IsRequestValid(managerRequest))
                 .Bind(AddManager)
                 .Bind(AddServiceSupplierAndRelation)
                 .Map(Build);


            async Task<bool> DoesManagerNotExist() => !await _managerContext.DoesManagerExist();


            async Task<Result<Common.Models.Manager>> AddManager()
            {
                return await Create()
                    .Bind(Add);


                Result<Common.Models.Manager> Create()
                {
                    var utcNowDate = DateTime.UtcNow;
                    return new Common.Models.Manager
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


                async Task<Result<Common.Models.Manager>> Add(Common.Models.Manager manager)
                {
                    var entry = _dbContext.Managers.Add(manager);
                    await _dbContext.SaveChangesAsync();
                    _dbContext.DetachEntry(entry.Entity);

                    return entry.Entity;
                }
            }


            async Task<Result<Common.Models.ManagerContext>> AddServiceSupplierAndRelation(Common.Models.Manager manager)
            {
                return await CreateServiceSupplier()
                    .Bind(AddServiceSupplier)
                    .Bind(serviceSupplier => CreateRelation(serviceSupplier))
                    .Bind(AddRelation);


                Result<Common.Models.ServiceSupplier> CreateServiceSupplier()
                {
                    var utcNowDate = DateTime.UtcNow;
                    return new Common.Models.ServiceSupplier
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


                async Task<Result<Common.Models.ServiceSupplier>> AddServiceSupplier(Common.Models.ServiceSupplier serviceSupplier)
                {
                    var entry = _dbContext.ServiceSuppliers.Add(serviceSupplier);
                    await _dbContext.SaveChangesAsync();
                    _dbContext.DetachEntry(entry.Entity);

                    return entry.Entity;
                }


                Result<Common.Models.ManagerServiceSupplierRelation> CreateRelation(Common.Models.ServiceSupplier serviceSupplier)
                {
                    return new Common.Models.ManagerServiceSupplierRelation
                    {
                        ManagerId = manager.Id,
                        ManagerPermissions = Common.Models.Enums.ManagerPermissions.All,
                        ServiceSupplierId = serviceSupplier.Id,
                        IsMaster = true,
                        IsActive = true
                    };
                }


                async Task<Result<Common.Models.ManagerContext>> AddRelation(Common.Models.ManagerServiceSupplierRelation managerRelation)
                {
                    var entry = _dbContext.ManagerServiceSupplierRelations.Add(managerRelation);
                    await _dbContext.SaveChangesAsync();
                    _dbContext.DetachEntry(entry.Entity);

                    return CollectManagerContext(manager, entry.Entity);
                }
            }
        }


        public Task<Result<Models.Responses.ManagerContext>> RegisterInvited(Models.Requests.ManagerInfo managerInfoRequest, string invitationCode, string email)
        {
            return CheckIdentityHashNotEmpty()
                .Bind(GetPendingInvitation)
                .Ensure(IsEmailUnique, "Manager with this email already exists")
                .Check(manager => IsRequestValid(managerInfoRequest))
                .Bind(AddManagerAndRelation)
                .Tap(LogSuccess)
                .Bind(GetMasterManager)
                .Bind(SendRegistrationMailToMaster)
                .OnFailure(LogFailed)
                .Map(Build);


            Task<Result<Models.Requests.ManagerInvitation>> GetPendingInvitation() => _managerInvitationService.GetPendingInvitation(invitationCode);


            async Task<bool> IsEmailUnique(Models.Requests.ManagerInvitation managerInvitation)
                => !await _dbContext.Managers.AnyAsync(manager => manager.Email == managerInvitation.Email);


            async Task<Result<ManagerContext>> AddManagerAndRelation(Models.Requests.ManagerInvitation managerInvitation)
            {
                return await (await BeginTransaction())
                    .Bind(transaction => AddManager(transaction, managerInvitation))
                    .Bind(AddManagerRelation)
                    .Tap(AcceptInvitation)
                    .Finally(EndTransaction);


                async Task<Result<IDbContextTransaction>> BeginTransaction()
                {
                    var transaction = _dbContext.Database.CurrentTransaction is null
                        ? await _dbContext.Database.BeginTransactionAsync()
                        : null;

                    return transaction is null
                        ? Result.Failure<IDbContextTransaction>("Failed to start transaction")
                        : Result.Success(transaction);
                }


                async Task<Result<(IDbContextTransaction transaction, Models.Requests.ManagerInvitation, Manager)>> AddManager(IDbContextTransaction transaction, 
                    Models.Requests.ManagerInvitation managerInvitation)
                {
                    var manager = new Manager
                    {
                        Title = managerInfoRequest.Title,
                        FirstName = managerInfoRequest.FirstName,
                        LastName = managerInfoRequest.LastName,
                        Position = managerInfoRequest.Position,
                        Email = email,
                        //IdentityHash = HashGenerator.ComputeSha256(externalIdentity),
                        Created = DateTime.UtcNow
                    };

                    _dbContext.Managers.Add(manager);
                    await _dbContext.SaveChangesAsync();

                    return Result.Success((transaction, managerInvitation, manager));
                }


                async Task<Result<(IDbContextTransaction, ManagerContext)>> AddManagerRelation((IDbContextTransaction, Models.Requests.ManagerInvitation, Manager) invitationData)
                {
                    var (transaction, managerInvitation, manager) = invitationData;

                    var managerRelation = await AddManagerServiceSupplierRelation(manager, ManagerPermissions.All, false, managerInvitation.ServiceSupplierId);

                    var managerContext = CollectManagerContext(manager, managerRelation);

                    return Result.Success((transaction, managerContext));
                }


                async Task AcceptInvitation()
                {
                    await _managerInvitationService.Accept(invitationCode);
                }


                async Task<Result<ManagerContext>> EndTransaction(Result<(IDbContextTransaction, ManagerContext)> invitationData)
                {
                    var (transaction, managerContext) = invitationData.Value;

                    try
                    {
                        if (invitationData.IsSuccess)
                        {
                            await transaction?.CommitAsync();

                            return Result.Success(managerContext);
                        }
                        else
                            return Result.Failure<ManagerContext>(invitationData.Error);
                    }
                    finally
                    {
                        transaction?.Dispose();
                    }
                }
            }


            void LogSuccess(ManagerContext managerContext)
            {
                _logger.LogManagerRegistrationSuccess($"Manager {email} successfully registered and bound to service supplier ID:'{managerContext.ServiceSupplierId}'");
            }


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


            void LogFailed(string error)
            {
                _logger.LogManagerRegistrationFailed(error);
            }
        }


        private Result CheckIdentityHashNotEmpty()
        {
            return string.IsNullOrEmpty(_managerContext.GetIdentityHash())
                ? Result.Failure("Manager should have identity")
                : Result.Success();
        }


        private Result IsRequestValid(Models.Requests.ManagerWithServiceSupplier managerRequest)
            => ValidationHelper.Validate(managerRequest, new ManagerWithServiceSupplierValidator());


        private Result IsRequestValid(Models.Requests.ManagerInfo managerInfoRequest)
            => ValidationHelper.Validate(managerInfoRequest, new ManagerInfoValidator());


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
        {
            return new ManagerContext
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
        }
        
        
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


        private readonly IManagerContextService _managerContext;
        private readonly IManagerInvitationService _managerInvitationService;
        private readonly INotificationService _notificationService;
        private readonly DirectContractsDbContext _dbContext;
        private readonly ILogger<ManagerRegistrationService> _logger;
    }
}