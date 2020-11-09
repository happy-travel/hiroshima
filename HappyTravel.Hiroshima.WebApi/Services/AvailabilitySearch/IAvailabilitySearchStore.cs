using System.Threading.Tasks;
using HappyTravel.EdoContracts.Accommodations;

namespace HappyTravel.Hiroshima.WebApi.Services.AvailabilitySearch
{
    public interface IAvailabilitySearchStore
    {
        Task AddAvailabilityRequest(string availabilityId, in AvailabilityRequest availabilityRequest);

        Task<AvailabilityRequest> GetAvailabilityRequest(string availabilityId);

        Task RemoveAvailabilityRequest(string availabilityId);
    }
}