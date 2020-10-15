using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.DirectManager.Models.Requests;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public interface IContractManagerManagementService
    {
        Task<Result<Models.Responses.ContractManager>> Get();
        
        Task<Result<Models.Responses.ContractManager>> Register(ContractManager contractManagerRequest, string email);

        Task<Result<Models.Responses.ContractManager>> Modify(Models.Requests.ContractManager contractManagerRequest);
    }
}