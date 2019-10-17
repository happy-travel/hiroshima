using System.Collections.Generic;
using System.Threading.Tasks;
using Hiroshima.DirectContracts.Models;

namespace Hiroshima.DirectContracts.Services
{
    public interface IDcLocations
    {
        Task<List<SearchLocation>> GetLocations();
    }
}
