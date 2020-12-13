using CSharpFunctionalExtensions;
using System;
using System.Threading.Tasks;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public class ManagerInvitationService : IManagerInvitationService
    {
        public Task Accept(string invitationCode)
        {
            throw new NotImplementedException();
        }


        public Task<Result<Models.Requests.ManagerInvitation>> GetPendingInvitation(string invitationCode)
        {
            throw new NotImplementedException();
            /*            return GetInvitation(invitationCode).ToResult("Could not find invitation")
                            .Ensure(IsNotAccepted, "Already accepted")
                            .Ensure(IsNotResent, "Already resent")
                            .Ensure(HasCorrectType, "Invitation type mismatch")
                            .Ensure(InvitationIsActual, "Invitation expired")
                            .Map(GetInvitationData<TInvitationData>);

                        static bool IsNotAccepted(InvitationBase invitation) => !invitation.IsAccepted;

                        static bool IsNotResent(InvitationBase invitation) => invitation.InvitationType switch
                        {
                            UserInvitationTypes.Agent => !((AgentInvitation)invitation).IsResent,
                            UserInvitationTypes.Administrator => true,
                            _ => throw new NotImplementedException($"{Formatters.EnumFormatters.FromDescription(invitation.InvitationType)} not supported")
                        };

                        bool HasCorrectType(InvitationBase invitation) => invitation.InvitationType == invitationType;

                        bool InvitationIsActual(InvitationBase invitation) => invitation.Created + _options.InvitationExpirationPeriod > _dateTimeProvider.UtcNow();*/
        }


        /*private async Task<Maybe<InvitationBase>> GetInvitation(string code)
        {
            var invitation = await _context.UserInvitations
                .SingleOrDefaultAsync(c => c.CodeHash == HashGenerator.ComputeSha256(code));

            return invitation ?? Maybe<InvitationBase>.None;
        }*/
    }
}
