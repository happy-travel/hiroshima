using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public interface IContractManagerManagementService
    {
        Task<Result<Models.Responses.Manager>> Get();
        
        Task<Result<Models.Responses.Manager>> Register(Models.Requests.Manager contractManagerRequest, string email);

        Task<Result<Models.Responses.Manager>> Modify(Models.Requests.Manager contractManagerRequest);
    }
}