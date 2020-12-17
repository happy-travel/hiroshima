using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.DirectManager.Models.Requests;
using System.Threading.Tasks;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public interface INotificationService
    {
        Task<Result> SendInvitation(ManagerInvitationInfo managerInvitation, string serviceSupplierName);

        Task<Result> SendRegistrationConfirmation(string emailMasterManager, ManagerInfo managerInfo, string serviceSupplierName);
    }
}
