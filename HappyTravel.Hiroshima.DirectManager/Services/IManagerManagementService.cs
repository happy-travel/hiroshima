using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public interface IManagerManagementService
    {
        Task<Result<Models.Responses.Manager>> Get();

        Task<Result<Models.Responses.Manager>> Get(int managerId);
        
        Task<Result<Models.Responses.Manager>> Register(Models.Requests.Manager managerRequest, string email);

        Task<Result<Models.Responses.Company>> RegisterCompany(Models.Requests.Company companyRequest);

        Task<Result<Models.Responses.Manager>> Modify(Models.Requests.Manager managerRequest);

        Task<Result<Models.Responses.Manager>> ModifyPermissions(int managerId, Models.Requests.ManagerPermissions managerPermissionsRequest);
    }
}