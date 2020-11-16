using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.EdoContracts.GeoData.Enums;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public interface ILocationManagementService
    {
        Task<Result<Models.Responses.Location>> Add(Models.Requests.Location location);
        
        Task<List<Models.Responses.Location>> Get(int top, int skip);

        Task<List<EdoContracts.GeoData.Location>> Get(DateTime modified, LocationTypes locationType, int skip = 0, int take = 10000);
        
        Task<List<string>> GetCountryNames(int top, int skip);
    }
}