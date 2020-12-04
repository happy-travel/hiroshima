using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Common.Models;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public interface IManagerContextService
    {
        Task<Result<Manager>> GetManager();

        string GetIdentityHash();

        Task<bool> DoesManagerExist();

        Task<Result<ServiceSupplier>> GetServiceSupplier(Manager manager);
    }
}