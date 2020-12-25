using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public interface IManagerManagementService
    {
        Task<Result<Models.Responses.ManagerContext>> Get();

        Task<Result<Models.Responses.ManagerContext>> Get(int managerId);

        Task<Result<List<Models.Responses.ServiceSupplier>>> GetServiceSuppliers();

        Task<Result<Models.Responses.ManagerContext>> Modify(Models.Requests.Manager managerRequest);

        Task<Result<Models.Responses.ManagerContext>> ModifyPermissions(int managerId, Models.Requests.Permissions managerPermissionsRequest);

        Task<Result<Models.Responses.ServiceSupplier>> ModifyServiceSupplier(Models.Requests.ServiceSupplier companyRequest);

        Task<Result> TransferMaster(int managerId);
    }
}