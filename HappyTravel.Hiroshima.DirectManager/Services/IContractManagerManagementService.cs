using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public interface IContractManagerManagementService
    {
        Task<Result<Models.Responses.ContractManager>> Get();
        
        Task<Result<Models.Responses.ContractManager>> Register(Models.Requests.ContractManager contractManagerRequest, string email);

        Task<Result<Models.Responses.ContractManager>> Modify(Models.Requests.ContractManager contractManagerRequest);
    }
}