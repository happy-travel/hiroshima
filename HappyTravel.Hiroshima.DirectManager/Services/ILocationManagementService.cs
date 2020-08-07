using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public interface ILocationManagementService
    {
        Task<Result<Models.Responses.Location>> GetOrAdd(string countryName, string localityName);

        Task<List<Models.Responses.Location>> Get(int take, int skip);
        Task<List<string>> GetCountryNames(int take, int skip);
    }
}