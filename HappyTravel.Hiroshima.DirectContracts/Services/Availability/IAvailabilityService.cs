using System.Threading.Tasks;

namespace HappyTravel.Hiroshima.DirectContracts.Services.Availability
{
    public interface IAvailabilityService
    {
        Task<Common.Models.Availabilities.Availability> Get(EdoContracts.Accommodations.AvailabilityRequest availabilityRequest, string languageCode);

        Task<Common.Models.Availabilities.Availability> Get(EdoContracts.Accommodations.AvailabilityRequest availabilityRequest, int accommodationId, string languageCode);
    }
}