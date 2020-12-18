using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Common.Models;
using System.Threading.Tasks;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public interface INotificationService
    {
        Task<Result> SendInvitation(ManagerInvitation managerInvitation, string serviceSupplierName);

        Task<Result> SendRegistrationConfirmation(string emailMasterManager, Models.Requests.ManagerInfo managerInfo, string serviceSupplierName);
    }
}
