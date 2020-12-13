using CSharpFunctionalExtensions;
using System.Threading.Tasks;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public interface IManagerInvitationService
    {
        Task Accept(string invitationCode);

        Task<Result<Models.Requests.ManagerInvitation>> GetPendingInvitation(string invitationCode);


    }
}
