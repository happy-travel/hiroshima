using System.Collections.Generic;
using System.Threading.Tasks;
using HappyTravel.EdoContracts.GeoData;

namespace Hiroshima.DirectContracts.Services
{
    public interface IDirectContractsLocationService
    {
        Task<List<Location>> GetLocations();
    }
}