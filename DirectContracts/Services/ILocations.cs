using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Hiroshima.DirectContracts.Models.Responses;

namespace Hiroshima.DirectContracts.Services
{
    public interface ILocations
    {
        Task<List<DcSearchLocation>> GetLocations();
    }
}
