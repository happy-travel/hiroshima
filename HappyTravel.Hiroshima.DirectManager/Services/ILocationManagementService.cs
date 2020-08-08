using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public interface ILocationManagementService
    {
        Task<Result<Models.Responses.Location>> GetOrAdd(Models.Requests.Location location);

        Task<List<Models.Responses.Location>> Get(int top, int skip);
        Task<List<string>> GetCountryNames(int top, int skip);
    }
}