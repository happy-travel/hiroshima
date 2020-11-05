using System.Threading.Tasks;
using HappyTravel.EdoContracts.Accommodations;

namespace HappyTravel.Hiroshima.WebApi.Services.AvailabilitySearch
{
    public interface IAvailabilitySearchStore
    {
        Task Add(Availability availability);

        Task<Availability> Get(string availabilityId);
    }
}