using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Common.Models.Enums;
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.Data.Extensions;
using HappyTravel.Hiroshima.DirectManager.RequestValidators;
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
                //_logger.LogAgentRegistrationSuccess($"Manager {email} successfully registered and bound to service supplier ID:'{managerContext.ServiceSupplierId}'");
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
                //_logger.LogManagerRegistrationFailed(error);
            }
        }


        private Result CheckIdentityHashNotEmpty()
        {
            return string.IsNullOrEmpty(_managerContext.GetIdentityHash())
                ? Result.Failure("Manager should have identity")
                : Result.Success();
        }


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


        private ManagerContext CollectManagerContext(Manager manager, ManagerServiceSupplierRelation managerRelation)
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