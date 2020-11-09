using System.Threading.Tasks;
using HappyTravel.EdoContracts.Accommodations;

namespace HappyTravel.Hiroshima.WebApi.Services.AvailabilitySearch
{
    public interface IAvailabilitySearchStore
    {
        Task AddAvailabilityRequest(string availabilityId, in AvailabilityRequest availabilityRequest);

        Task AddAccommodationAvailability(AccommodationAvailability accommodationAvailability);
        
        Task<AvailabilityRequest> GetAvailabilityRequest(string availabilityId);

        Task<AccommodationAvailability> GetAccommodationAvailability(string availabilityId);
        
        Task RemoveAvailability(string availabilityId);
    }
}