using CSharpFunctionalExtensions;
using System.Threading.Tasks;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public interface IManagerRegistrationService
    {
        Task<Result<Models.Responses.ManagerContext>> RegisterInvited(Models.Requests.ManagerInfo managerInfo, string invitationCode, string email);
    }
}