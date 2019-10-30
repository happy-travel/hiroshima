using System.Collections.Generic;
using System.Threading.Tasks;
using HappyTravel.EdoContracts.GeoData;

namespace Hiroshima.DirectContracts.Services
{
    public interface IDirectContractsLocation
    {
        Task<List<Location>> GetLocations();
    }
}
