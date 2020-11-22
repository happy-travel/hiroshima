using System.Threading.Tasks;

namespace HappyTravel.Hiroshima.DirectContracts.Services.Availability
{
    public interface IAvailabilityService
    {
        Task<Models.Availability> Get(EdoContracts.Accommodations.AvailabilityRequest availabilityRequest, string languageCode);

        Task<Models.Availability> Get(EdoContracts.Accommodations.AvailabilityRequest availabilityRequest, int accommodationId, string languageCode);
    }
}