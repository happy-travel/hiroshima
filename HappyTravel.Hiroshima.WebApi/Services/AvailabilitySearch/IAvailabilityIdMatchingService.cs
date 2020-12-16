using System.Threading.Tasks;

namespace HappyTravel.Hiroshima.WebApi.Services.AvailabilitySearch
{
    public interface IAvailabilityIdMatchingService
    {
        Task SetAccommodationAvailabilityId(string wideAvailabilityId, string accommodationAvailabilityId);

        Task<string> GetAccommodationAvailabilityId(string wideAvailability);
    }
}