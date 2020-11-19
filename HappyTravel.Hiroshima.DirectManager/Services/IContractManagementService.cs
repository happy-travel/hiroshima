using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.DirectManager.Models.Responses;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public interface IContractManagementService
    {
        Task<Result<Contract>> Get(int contractId);
        Task<Result<List<Contract>>> GetContracts(int skip = 0, int top = 100);
        Task<Result<Contract>> Add(Models.Requests.Contract contract);
        Task<Result> Update(int contractId, Models.Requests.Contract contract);
        Task<Result> Remove(int contractId);
    }
}