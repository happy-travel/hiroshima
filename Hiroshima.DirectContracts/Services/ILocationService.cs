using System.Collections.Generic;
using System.Threading.Tasks;
using HappyTravel.EdoContracts.GeoData;

namespace Hiroshima.DirectContracts.Services
{
    public interface ILocationService
    {
        Task<List<Location>> GetLocations();
    }
}