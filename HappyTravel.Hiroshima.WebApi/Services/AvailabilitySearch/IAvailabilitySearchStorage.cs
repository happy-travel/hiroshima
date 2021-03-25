using System.Threading.Tasks;
using HappyTravel.EdoContracts.Accommodations;

namespace HappyTravel.Hiroshima.Api.Services.AvailabilitySearch
{
    public interface IAvailabilitySearchStorage
    {
        Task AddAvailabilityRequest(string availabilityId, in AvailabilityRequest availabilityRequest);

        Task AddAccommodationAvailability(in AccommodationAvailability accommodationAvailability);

        Task RemoveAvailabilityRequest(string availabilityId);
        
        Task<AvailabilityRequest> GetAvailabilityRequest(string availabilityId);

        Task<AccommodationAvailability> GetAccommodationAvailability(string availabilityId);
        
        Task RemoveAvailability(string availabilityId);
    }
}