using CSharpFunctionalExtensions;
using System.Threading.Tasks;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public interface IManagerInvitationService
    {
        Task<Result> Send(Models.Requests.ManagerInvitationInfo managerInvitation);

        Task<Result<string>> Create(Models.Requests.ManagerInvitationInfo managerInvitation);

        Task<Result> Resend(string invitationCode);

        Task Accept(string invitationCode);

        Task<Result<Models.Responses.ManagerInvitation>> GetPendingInvitation(string invitationCode);
    }
}
