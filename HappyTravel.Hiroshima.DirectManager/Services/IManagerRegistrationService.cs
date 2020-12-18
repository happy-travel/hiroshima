using CSharpFunctionalExtensions;
using System.Threading.Tasks;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public interface IManagerRegistrationService
    {
        Task<Result<Models.Responses.ManagerContext>> RegisterWithServiceSupplier(Models.Requests.ManagerWithServiceSupplier managerWithServiceSupplierRequest, string email);

        Task<Result<Models.Responses.ManagerContext>> RegisterInvited(Models.Requests.ManagerInfoWithCode managerInfo, string email);
    }
}