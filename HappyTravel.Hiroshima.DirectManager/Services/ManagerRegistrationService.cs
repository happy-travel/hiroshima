using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Common.Models.Enums;
using HappyTravel.Hiroshima.Data;
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
                //.BindWithTransaction(_dbContext, invitation => Result.Success(invitation)
                //    .Bind(AddManager)
                //    .Tap(AddManagerRelation)
                //    .Map(AcceptInvitation))
                .Bind(AddManagerAndRelation)
                .Bind(LogSuccess)
                .Bind(GetMasterManager)
                .Bind(SendRegistrationMailToMaster)
                .OnFailure(LogFailed);


            Task<Result<Models.Requests.ManagerInvitation>> GetPendingInvitation() => _managerInvitationService.GetPendingInvitation(invitationCode);


            async Task<bool> IsEmailUnique(Models.Requests.ManagerInvitation managerInvitation)
                => !await _dbContext.Managers.AnyAsync(manager => manager.Email == managerInvitation.Email);

            async Task<Result<Models.Requests.ManagerInvitation>> AddManagerAndRelation(Models.Requests.ManagerInvitation managerInvitation)
            {
                return (await BeginTransaction())
                    .Bind(AddManager)
                    .Tap(AddManagerRelation)
                    .Map(AcceptInvitation)
                    .EndTransaction();

            }


            async Task<Result<IDbContextTransaction>> BeginTransaction()
            {
                var transaction = _dbContext.Database.CurrentTransaction is null
                    ? await _dbContext.Database.BeginTransactionAsync()
                    : null;

                return transaction is null
                    ? Result.Failure<IDbContextTransaction>("Failed to start transaction")
                    : Result.Success(transaction);
            }
            

            async Task EndTransaction(Result result, IDbContextTransaction transaction)
            {
                try
                {
                    if (result.IsSuccess)
                        await transaction?.CommitAsync();

                    //return result;
                }
                finally
                {
                    transaction?.Dispose();
                }
            }


            async Task<Result<(Models.Requests.ManagerInvitation, Common.Models.Manager)>> AddManager(Models.Requests.ManagerInvitation managerInvitation)
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

                return Result.Success((managerInvitation, manager));
            }


            async Task AddManagerRelation((Models.Requests.ManagerInvitation, Common.Models.Manager) invitationData)
            {
                var (managerInvitation, manager) = invitationData;

                await AddManagerServiceSupplierRelation(manager, ManagerPermissions.All, false, managerInvitation.ServiceSupplierId);
            }


            async Task<Models.Requests.ManagerInvitation> AcceptInvitation((Models.Requests.ManagerInvitation managerInvitation, Common.Models.Manager manager) invitationData)
            {
                await _managerInvitationService.Accept(invitationCode);

                return invitationData.managerInvitation;
            }


            void LogFailed(string error)
            {
                //_logger.LogManagerRegistrationFailed(error);
            }


            Result<Models.Requests.ManagerInvitation> LogSuccess(Models.Requests.ManagerInvitation managerInvitation)
            {
                //_logger.LogAgentRegistrationSuccess($"Manager {email} successfully registered and bound to service supplier ID:'{managerInvitation.ServiceSupplierId}'");
                return Result.Success(managerInvitation);
            }


            Task<Result<Common.Models.Manager>> GetMasterManager(Models.Requests.ManagerInvitation managerInvitation)
                => _managerContext.GetMasterManager(managerInvitation.ServiceSupplierId);


            async Task<Result> SendRegistrationMailToMaster(Common.Models.Manager masterManager)
            {
                var serviceSupplier = await _dbContext.ServiceSuppliers.SingleOrDefaultAsync(serviceSupplier => serviceSupplier.Id == 1);   // TODO: Need id service supplier
                if (serviceSupplier is null)
                    return Result.Failure("Service supplier not found");

                return await _notificationService.SendRegistrationConfirmation(masterManager.Email, managerInfoRequest, serviceSupplier.Name);
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


        private Task AddManagerServiceSupplierRelation(Manager manager, ManagerPermissions managerPermissions, bool isMaster, int serviceSupplierId)
        {
            _dbContext.ManagerServiceSupplierRelations.Add(new ManagerServiceSupplierRelation
            {
                ManagerId = manager.Id,
                ManagerPermissions = managerPermissions,
                IsMaster = isMaster,
                ServiceSupplierId = serviceSupplierId,
                IsActive = true
            });

            return _dbContext.SaveChangesAsync();
        }


        private readonly IManagerContextService _managerContext;
        private readonly IManagerInvitationService _managerInvitationService;
        private readonly INotificationService _notificationService;
        private readonly DirectContractsDbContext _dbContext;
        private readonly ILogger<ManagerRegistrationService> _logger;
    }
}
