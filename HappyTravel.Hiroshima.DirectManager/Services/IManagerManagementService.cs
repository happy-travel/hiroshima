using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public interface IManagerManagementService
    {
        Task<Result<Models.Responses.ManagerContext>> Get();

        Task<Result<Models.Responses.ManagerContext>> Get(int managerId);
        
        Task<Result<Models.Responses.ManagerContext>> Register(Models.Requests.Manager managerRequest, string email);

        Task<Result<Models.Responses.ServiceSupplier>> RegisterServiceSupplier(Models.Requests.ServiceSupplier companyRequest);

        Task<Result<Models.Responses.ManagerContext>> Modify(Models.Requests.Manager managerRequest);

        Task<Result<Models.Responses.ManagerContext>> ModifyPermissions(int managerId, Models.Requests.ManagerPermissions managerPermissionsRequest);
    }
}