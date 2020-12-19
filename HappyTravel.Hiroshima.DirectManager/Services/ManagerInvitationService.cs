using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.Data.Extensions;
using HappyTravel.Hiroshima.DirectContracts.Extensions.FunctionalExtensions;
using HappyTravel.Hiroshima.DirectManager.Infrastructure.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public class ManagerInvitationService : IManagerInvitationService
    {
        public ManagerInvitationService(IManagerContextService managerContextService, INotificationService notificationService, 
            DirectContractsDbContext dbContext, ILogger<ManagerInvitationService> logger, IOptions<ManagerInvitationOptions> options)
        {
            _managerContext = managerContextService;
            _notificationService = notificationService;
            _dbContext = dbContext;
            _logger = logger;
            _options = options;
        }


        public async Task<Result> Send(Models.Requests.ManagerInvitationInfo managerInvitationInfo)
        {
            return await _managerContext.GetManagerRelation()
                .Ensure(managerRelation => HasManagerInvitationManagerPermission(managerRelation).Value, "The manager does not have enough rights")
                .Bind(GenerateInvitationCode)
                .BindWithTransaction(_dbContext, managerData => Result.Success(managerData)
                    .Bind(SaveInvitation)
                    .Bind(SendInvitationMail))
                .Tap(managerInvitation => LogInvitationCreated(managerInvitationInfo.Email));


            async Task<Result<ManagerInvitation>> SaveInvitation((ManagerServiceSupplierRelation, string) invitationData)
            {
                var (managerRelation, invitationCode) = invitationData;

                var managerInvitation = new ManagerInvitation
                {
                    InvitationCode = invitationCode,
                    FirstName = managerInvitationInfo.FirstName,
                    LastName = managerInvitationInfo.LastName,
                    Title = managerInvitationInfo.Title,
                    Position = managerInvitationInfo.Position,
                    Email = managerInvitationInfo.Email,
                    ManagerId = managerRelation.ManagerId,
                    ServiceSupplierId = managerRelation.ServiceSupplierId,
                    Created = DateTime.UtcNow,
                    IsAccepted = false,
                    IsResent = false
                };

                var entry = _dbContext.ManagerInvitations.Add(managerInvitation);
                await _dbContext.SaveChangesAsync();
                _dbContext.DetachEntry(entry.Entity);

                return managerInvitation;
            }
        }


        public async Task<Result<string>> Create(Models.Requests.ManagerInvitationInfo managerInvitationInfo)
        {
            return await _managerContext.GetManagerRelation()
                .Ensure(managerRelation => HasManagerInvitationManagerPermission(managerRelation).Value, "The manager does not have enough rights")
                .Bind(GenerateInvitationCode)
                .Bind(SaveInvitation)
                .Tap(invitationCode => LogInvitationCreated(managerInvitationInfo.Email));


            async Task<Result<string>> SaveInvitation((ManagerServiceSupplierRelation, string) invitationData)
            {
                var (managerRelation, invitationCode) = invitationData;

                var managerInvitation = new ManagerInvitation
                {
                    InvitationCode = invitationCode,
                    FirstName = managerInvitationInfo.FirstName,
                    LastName = managerInvitationInfo.LastName,
                    Title = managerInvitationInfo.Title,
                    Position = managerInvitationInfo.Position,
                    Email = managerInvitationInfo.Email,
                    ManagerId = managerRelation.ManagerId,
                    ServiceSupplierId = managerRelation.ServiceSupplierId,
                    Created = DateTime.UtcNow,
                    IsAccepted = false,
                    IsResent = false
                };

                var entry = _dbContext.ManagerInvitations.Add(managerInvitation);
                await _dbContext.SaveChangesAsync();
                _dbContext.DetachEntry(entry.Entity);

                return invitationCode;
            }
        }


        public async Task<Result> Resend(string invitationCode)
        {
            return await _managerContext.GetManagerRelation()
                .Ensure(managerRelation => HasManagerInvitationManagerPermission(managerRelation).Value, "The manager does not have enough rights")
                .Bind(managerRelation => GetExistingInvitation())
                .Bind(SendInvitationMail)
                .Bind(DisableExistingInvitation);


            async Task<Result<ManagerInvitation>> GetExistingInvitation()
            {
                var invitation = await _dbContext.ManagerInvitations.SingleOrDefaultAsync(i => i.InvitationCode == invitationCode);

                return invitation ?? Result.Failure<ManagerInvitation>($"Invitation with Code {invitationCode} not found");
            }


            async Task<Result> DisableExistingInvitation(ManagerInvitation existingInvitation)
            {
                existingInvitation.IsResent = true;
                await _dbContext.SaveChangesAsync();
                return Result.Success();
            }
        }


        public async Task Accept(string invitationCode)
        {
            var invitationMaybe = await GetInvitation(invitationCode);
            if (invitationMaybe.HasValue)
            {
                var managerInvitation = invitationMaybe.Value;
                managerInvitation.IsAccepted = true;

                _dbContext.Update(managerInvitation);
                await _dbContext.SaveChangesAsync();
            }
        }


        public Task<Result<Models.Responses.ManagerInvitation>> GetPendingInvitation(string invitationCode)
        {
            return GetInvitation(invitationCode).ToResult("Could not find invitation")
                .Ensure(IsNotAccepted, "Invitation already accepted")
                .Ensure(IsNotResent, "Invitation already resent")
                .Ensure(InvitationIsActual, "Invitation expired")
                .Map(Build);


            static bool IsNotAccepted(ManagerInvitation managerInvitation) => !managerInvitation.IsAccepted;


            static bool IsNotResent(ManagerInvitation managerInvitation) => !managerInvitation.IsResent;


            bool InvitationIsActual(ManagerInvitation managerInvitation) 
                => managerInvitation.Created + _options.Value.InvitationExpirationPeriod > DateTime.UtcNow;


            Models.Responses.ManagerInvitation Build(ManagerInvitation managerInvitation)
            {
                return new Models.Responses.ManagerInvitation(managerInvitation.FirstName, 
                    managerInvitation.LastName, 
                    managerInvitation.Title,
                    managerInvitation.Position,
                    managerInvitation.Email,
                    managerInvitation.ManagerId,
                    managerInvitation.ServiceSupplierId);
            }
        }


        private Result<(ManagerServiceSupplierRelation, string)> GenerateInvitationCode(ManagerServiceSupplierRelation managerInvitation)
        {
            using var provider = new RNGCryptoServiceProvider();

            var byteArray = new byte[64];
            provider.GetBytes(byteArray);

            return (managerInvitation, Base64UrlEncoder.Encode(byteArray));
        }


        private async Task<Maybe<ManagerInvitation>> GetInvitation(string invitationCode)
        {
            var managerInvitation = await _dbContext.ManagerInvitations.SingleOrDefaultAsync(c => c.InvitationCode == invitationCode);

            return managerInvitation ?? Maybe<ManagerInvitation>.None;
        }


        private async Task<Result<ManagerInvitation>> SendInvitationMail(ManagerInvitation managerInvitation)
        {
            var serviceSupplier = await _managerContext.GetServiceSupplier();
            if (serviceSupplier.IsFailure)
                return Result.Failure<ManagerInvitation>(serviceSupplier.Error);

            var sendingResult = await _notificationService.SendInvitation(managerInvitation, serviceSupplier.Value.Name);
            if (sendingResult.IsFailure)
                return Result.Failure<ManagerInvitation>(sendingResult.Error);

            return Result.Success(managerInvitation);
        }


        private void LogInvitationCreated(string email)
        {
            _logger.LogInvitationCreated($"The invitation created for the manager with email '{email}'");
        }


        private static Result<bool> HasManagerInvitationManagerPermission(ManagerServiceSupplierRelation managerRelation)
            => (managerRelation.ManagerPermissions & Common.Models.Enums.ManagerPermissions.ManagerInvitation) == Common.Models.Enums.ManagerPermissions.ManagerInvitation;


        private readonly IManagerContextService _managerContext;
        private readonly INotificationService _notificationService;
        private readonly DirectContractsDbContext _dbContext;
        private readonly ILogger<ManagerInvitationService> _logger;
        private readonly IOptions<ManagerInvitationOptions> _options;
    }
}