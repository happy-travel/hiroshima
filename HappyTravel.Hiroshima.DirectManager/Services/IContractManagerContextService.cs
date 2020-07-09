using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Data.Models;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public interface IContractManagerContextService
    {
        Task<Result<ContractManager>> GetContractManager();
    }
}